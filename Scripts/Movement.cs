using Godot;
using System;


public class Movement : Spatial
{
	public RigidBody ball;
	public MeshInstance Car_mesh;
	public RayCast rayCast;
	[Export] public string Ball_path;
	[Export] public string car_mesh_body_path;
	[Export] public string Car_mesh_path;
	[Export] public string rayCast_path; 
	[Export] public Vector3 sphere_offset = new Vector3(0, -1, 0);
	[Export] public float acceleration = 50;
	[Export] public float steering = 50;
	[Export] public float turn_speed = 5;
	[Export] public float turn_stop_limit = 0.75f;
	[Export] public float tilt = 35;
 	public float speed_input;
	public float steering_input;
	[Export] public string left_wheel_path;
	[Export] public string right_wheel_path;
	public MeshInstance car_mesh_body;
	public MeshInstance left_wheel;
	public MeshInstance right_wheel;
	[Export] public string B_L_particles;
	[Export] public string B_R_particles;
	public CPUParticles B_L;
	public CPUParticles B_R;
	[Export] public string button_path;
	[Export] public float damage_multiplier = 1;
	public Button button;
	[Export] public float health = 100;
	public bool Is_on_ramp = false;
	[Export] public float ramp_speed = 3;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ball = GetNode<RigidBody>(Ball_path);
		Car_mesh = GetNode<MeshInstance>(Car_mesh_path);
		rayCast = GetNode<RayCast>(rayCast_path);
		rayCast.AddException(ball);
		left_wheel = GetNode<MeshInstance>(left_wheel_path);
		right_wheel = GetNode<MeshInstance>(right_wheel_path);
		car_mesh_body = GetNode<MeshInstance>(car_mesh_body_path);
		B_L = GetNode<CPUParticles>(B_L_particles);
		B_R = GetNode<CPUParticles>(B_R_particles);
		button = GetParent().GetNode<Button>(button_path);
		
	}

	public override void _PhysicsProcess(float delta)
	{
		// align mesh with sphere
		var transform = Car_mesh.Transform;
		transform.origin = ball.Transform.origin + sphere_offset;
		Car_mesh.Transform = transform;
		// Acceleration
		speed_input = 0;
		speed_input += Input.GetActionStrength("ui_up");
		speed_input -=  Input.GetActionStrength("ui_down");
		// speed_input = -Input.GetAccelerometer().Normalized().y;
		
		// if (button.Pressed)
		// {
		// 	speed_input = 1;
			
		// }
		// else
		// {
		// 	speed_input = 0;
		// }
		speed_input = Mathf.Lerp(speed_input , speed_input*acceleration , delta * 25);

		//Steering 
		steering_input = 0;
		steering_input -= Input.GetActionStrength("ui_right");
		steering_input += Input.GetActionStrength("ui_left");
		// steering_input = -Input.GetAccelerometer().Normalized().x;
		steering_input *= Mathf.Deg2Rad(steering);

		//Accelerate
		if(Is_on_ramp)
		{
			speed_input *= ramp_speed;
		}
		ball.AddCentralForce(-Car_mesh.GlobalTransform.basis.z * speed_input);
		// Smoke
		var ball_velocity = ball.LinearVelocity.Normalized();
		var car_mesh_forward = Car_mesh.GlobalTransform.basis.z.Normalized();
		var dot_product = ball_velocity.Dot(car_mesh_forward);
		
		if(rayCast.IsColliding() && ball.LinearVelocity.Length() >16)
		{
			
			if(dot_product > 0)
			{
				B_L.Emitting = false;
				B_R.Emitting = false;
			}
			else 
			{
				B_L.Emitting = true;
				B_R.Emitting = true;
			}
			
		}
		else
		{
			B_L.Emitting = false;
			B_R.Emitting = false;
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		

		// set button pos
		var screen_size = OS.GetScreenSize();
		button.RectPosition = new Vector2(screen_size.x - button.RectSize.x - 150, screen_size.y - button.RectSize.y - 150);
		
		// turning wheels
		
		var right_rotation = right_wheel.Rotation;
		right_rotation.y = steering_input;
		var left_rotation = left_wheel.Rotation;
		left_rotation.y = 3.141593f + steering_input;
		left_wheel.Rotation = left_rotation;
		right_wheel.Rotation = right_rotation;
		// Apply steering
		if(ball.LinearVelocity.Length() > turn_stop_limit)
		{
			if(speed_input < 0)
			{
				steering_input = -steering_input;
			}
			var new_basis = Car_mesh.GlobalTransform.basis.Rotated(Car_mesh.GlobalTransform.basis.y ,steering_input );
			var transform = Car_mesh.GlobalTransform;
			transform.basis = Car_mesh.GlobalTransform.basis.Slerp(new_basis, turn_speed * delta);
			Car_mesh.GlobalTransform = transform.Orthonormalized();
			// Applying tilt
			var t = -steering_input * ball.LinearVelocity.Length() / tilt;
			var rotation = car_mesh_body.Rotation;
			rotation.z = Mathf.Lerp(rotation.z , t , 10 * delta);
			car_mesh_body.Rotation = rotation;
		}
		// Align with surface
		var n = rayCast.GetCollisionNormal().Normalized();
		var xform = Alignwithsurface(Car_mesh.GlobalTransform ,n);
		Car_mesh.GlobalTransform = Car_mesh.GlobalTransform.InterpolateWith(xform , turn_speed * 2 * delta);

	}
	public Transform Alignwithsurface(Transform xform ,Vector3 new_y)
	{
		xform.basis.y = new_y;
		xform.basis.x = -xform.basis.z.Cross(new_y);
		xform.basis = xform.basis.Orthonormalized();
		return xform;
		
	}
	public void On_collision(Node body)
	{
		if(body.IsInGroup("Ramp"))
		{
			Is_on_ramp = true;
		}
		if(body is RigidBody)	
		{
			// get relative velocity
			var col_body = body as RigidBody;
			var relative_velocity = col_body.LinearVelocity - ball.LinearVelocity;
			var Impact_magnitude = relative_velocity.Length();

			// Applying damage
			var damage = Calculate_Damage(Impact_magnitude);
			Apply_Damage(damage);
		}
	}
	public void On_leaving(Node body)
	{
		if(body.IsInGroup("Ramp"))
		{
			Is_on_ramp = false;
		}
	}
	public void Apply_Damage(float damage)
	{
		GD.Print("damage = " + damage);
		GD.Print("health = " + health);
		health -= damage;
		if(health <= 0)
		{
			GD.Print("DEAD");
		}
	}
	public float Calculate_Damage(float impact_magnitude)
	{
		var damage = Mathf.RoundToInt(impact_magnitude * damage_multiplier);
		return damage;
	}
}
