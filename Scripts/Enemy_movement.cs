using Godot;
using System;
using System.ComponentModel;

public class Enemy_movement : Spatial
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

    // AI components
	public MeshInstance player_mesh;
	[Export] public string player_mesh_path;
	[Export] public float avoidance_strength = 1;
	[Export] public string player_collider_path;
	public Vector3 direction;
	public RayCast player_collider;
	
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
		player_mesh = GetParent().GetNode<MeshInstance>(player_mesh_path);
		player_collider = GetNode<RayCast>(player_collider_path);
		player_collider.Enabled = true;
		player_collider.AddException(ball);
		player_collider.AddException(player_mesh);
		player_collider.CastTo = player_mesh.GlobalTransform.origin;
	}
	public override void _PhysicsProcess(float delta)
	{
		// align mesh with sphere
		var transform = Car_mesh.Transform;
		transform.origin = ball.Transform.origin + sphere_offset;
		Car_mesh.Transform = transform;
		//Accelerate
		ball.AddCentralForce(-Car_mesh.GlobalTransform.basis.z * speed_input);
		// GD.Print(speed_input);
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

		// AI
		direction = player_mesh.GlobalTransform.origin - car_mesh_body.GlobalTransform.origin;
		// GD.Print(new_direction);
		player_collider.Visible  = true;
		var dir_for_collider = new Vector3(direction.x , 0 , direction.y);
		player_collider.CastTo = dir_for_collider;
		if(player_collider.IsColliding())
		{
			var avoidance_vector = Calculate_Avoidance_vector(direction);
			GD.Print("Avoidance vector : " + avoidance_vector);
			direction += avoidance_vector;
			GD.Print(player_collider.GetCollider());
		}
		// Calculate angle
		var angle = Calculate_Angle(direction);
		// GD.Print("Angle : " + angle);
		if(angle > 20)
		{
			steering_input = Mathf.Lerp(steering_input , 1, delta*10 );
		}
		else if(angle < -20)
		{
			steering_input = Mathf.Lerp(steering_input , -1 , delta*10 );
		}
		else
		{
			steering_input = 0;
		}
		
		//Steering 
		// steering_input -= Input.GetActionStrength("ui_right");
		// steering_input += Input.GetActionStrength("ui_left");
		// steering_input = -Input.GetAccelerometer().Normalized().x;
		steering_input *= Mathf.Deg2Rad(steering);
		// Acceleration
		// speed_input = 0;
		// speed_input += Input.GetActionStrength("ui_up");
		// speed_input -=  Input.GetActionStrength("ui_down");
		// speed_input = -Input.GetAccelerometer().Normalized().y;
		
		var distance = direction.Length();
		if(distance > 2)
		{
			speed_input = 1.5f;
		}
		else
		{
			speed_input = 0;
		}
		speed_input = Mathf.Lerp(speed_input , speed_input*acceleration , delta * 25);
	}
	public Vector3 Calculate_Avoidance_vector(Vector3 direction)
	{
		// calculate the perpendicular vector for avoidance
		var perp_vector = direction.Cross(Vector3.Up).Normalized();
		GD.Print(perp_vector);
		// scaling the perpendicular vector(Changes required)
		perp_vector *= avoidance_strength;
		// return the perpendicular vector
		return perp_vector;

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		
		
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
			// turning wheels
		
			var right_rotation = right_wheel.Rotation;
			right_rotation.y = steering_input;
			var left_rotation = left_wheel.Rotation;
			left_rotation.y = 3.141593f + steering_input;
			left_wheel.Rotation = left_rotation;
			right_wheel.Rotation = right_rotation;
		}
		// Align with surface
		var n = rayCast.GetCollisionNormal().Normalized();
		var xform = Alignwithsurface(Car_mesh.GlobalTransform ,n);
		Car_mesh.GlobalTransform = Car_mesh.GlobalTransform.InterpolateWith(xform , turn_speed * 2 * delta);
		
	}
	
	public float Calculate_Angle(Vector3 direction)
	{
		// Calculate angle
		var angle = -car_mesh_body.GlobalTransform.basis.z.SignedAngleTo(direction , Vector3.Up);
		angle = Mathf.Rad2Deg(angle);
		return angle;
	}
	public Transform Alignwithsurface(Transform xform ,Vector3 new_y)
	{
		xform.basis.y = new_y;
		xform.basis.x = -xform.basis.z.Cross(new_y);
		xform.basis = xform.basis.Orthonormalized();
		return xform;
	}
}

