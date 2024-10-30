using Godot;
using System;


public class Movement : Spatial
{
	public RigidBody ball;
	public MeshInstance Car_mesh;
	public RayCast rayCast;
	[Export] public int Close_miss_bonus = 10;
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
	[Export] public string Accelerate_button_path;
	[Export] public float damage_multiplier = 1;
	public TouchScreenButton Accelerate_button;
	[Export] public float health = 100;
	public bool Is_on_ramp = false;
	[Export] public float ramp_speed = 3;
	[Signal] delegate void Change_Health(int health);
	[Export] public string health_bar_path;
	public TouchScreenButton Left;
	public TouchScreenButton Right;
	public TouchScreenButton Brake;
	public AudioStreamPlayer audioStreamPlayer;
	public Spatial health_bar;
	public AudioStreamPlayer3D drift;
	public float col_time = 0;
	public bool col = false;
	[Export] public float Jump_ht = 2.5f;
	public Control intrface;
	[Export] public int Drift_multiplier = 1;
	[Export] public int im = 88;
	public Camera cam;
	[Export]public NodePath min_map_cam_path;
	public Camera min_map_cam;
	public PackedScene exp;
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
		Accelerate_button = GetNode<TouchScreenButton>(Accelerate_button_path);
		health_bar = GetNode<Spatial>(health_bar_path);
		Connect("Change_Health", health_bar, nameof(Change_Health));
		audioStreamPlayer = GetNode<AudioStreamPlayer>("Ball/Oncollision");
		drift = GetNode<AudioStreamPlayer3D>("Spatial/Drift");
		Left = GetNode<TouchScreenButton>("Interface/Steering/Left");
		Right = GetNode<TouchScreenButton>("Interface/Steering/Right");
		Brake = GetNode<TouchScreenButton>("Interface/Acceleration/Brake");
		intrface = GetNode<Control>("Interface");
		cam = GetParent().GetNode<Camera>("Camera");
		min_map_cam = GetNode<Camera>(min_map_cam_path);
		exp = GD.Load<PackedScene>("res://Scenes/Explosion.tscn");
		var i = exp.Instance() as Spatial;
		GetTree().Root.AddChild(i);
		i.GlobalTranslation = ball.GlobalTranslation;

		// jump_timer = new Timer();
		// AddChild(jump_timer);
		// jump_timer.OneShot = true;
		// jump_timer.WaitTime = 0.1f;
		// jump_timer.Start();
	}

	public override void _PhysicsProcess(float delta)
	{
		min_map_cam.GlobalTranslation= new Vector3(ball.GlobalTranslation.x, min_map_cam.GlobalTranslation.y, ball.GlobalTranslation.z);
		// align mesh with sphere
		var transform = Car_mesh.Transform;
		transform.origin = ball.Transform.origin + sphere_offset;
		Car_mesh.Transform = transform;
		// Acceleration
		speed_input = 0;
		speed_input += Input.GetActionStrength("Up");
		speed_input -=  Input.GetActionStrength("Down");

		if (Accelerate_button.IsPressed())
		{
			speed_input = 1;
		}
		if(Brake.IsPressed())
		{
			speed_input = -1;
		}
		// else
		// {
		// 	speed_input = 0;
		// }
		speed_input = Mathf.Lerp(speed_input , speed_input*acceleration , delta * 25);

		//Steering 
		steering_input = 0;
		steering_input -= Input.GetActionStrength("Right");
		steering_input += Input.GetActionStrength("Left");
		if(Left.IsPressed())
		{
			steering_input = 1;
		}
		if(Right.IsPressed())
		{
			steering_input = -1;
		}
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
		var dot_product = -ball_velocity.Dot(car_mesh_forward);
		
		if(rayCast.IsColliding() && ball.LinearVelocity.Length() >13 && dot_product < 0.85 && dot_product > 0)
		{
			// GD.Print(dot_product);
			B_L.Emitting = true;
			B_R.Emitting = true;
			var points = ball.LinearVelocity.Length()/60*(1-dot_product)*Drift_multiplier;
			intrface.Call("AddDriftPoints",points);
			
			if(!drift.Playing)
			{
				//FIXME: Audio needs to be updated
				drift.Playing = true;
			}
		}
		else
		{
			B_L.Emitting = false;
			B_R.Emitting = false;
			drift.Playing = false;
		}
		if(col)
		{
			col_time+=delta;
		}
		else
		{
			col_time = 0;
		}
		if (col_time >= 0.1f)
		{
			health -= delta*2;
			EmitSignal("Change_Health", (int)health,true);
		}
	}
	public void Jump()
	{
		// if(jump_timer.TimeLeft == 0)
		// {
		// 	jump_timer.Start();
		// 	// ball.AddForce(new Vector3(0, Jump_ht*1000, 0), ball.GlobalTranslation);
		// 	// ball.ApplyImpulse( ball.GlobalTranslation,new Vector3(0, Jump_ht*50, 0));
		// 	ball.LinearVelocity += new Vector3(0, Jump_ht*50, 0);
		// 	GD.Print("JUMP");
		// }
		ball.LinearVelocity = new Vector3(ball.LinearVelocity.x, Jump_ht*40, ball.LinearVelocity.z);
		GD.Print("JUMP");
		cam.Call("Add_trauma",0.8f);
	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		//Screenshot
		if(Input.IsActionJustPressed("Save"))
		{
			var image = GetViewport().GetTexture().GetData();
			image.FlipY();
			image.SavePng("D:/Godot export/SS/World2/" + im + ".png");
			im++;
		}
		// turning wheels
		
		var left_rotation = left_wheel.Rotation;
		var right_rotation = right_wheel.Rotation;
		right_rotation.y = steering_input;
		left_rotation.y = 3.141593f + steering_input;
		if(B_L.Emitting)
		{
			left_rotation.y = -(3.341593f + steering_input);
			right_rotation.y = -(steering_input - .3f);
		}
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
		if(rayCast.IsColliding())
		{
			var xform = Alignwithsurface(Car_mesh.GlobalTransform ,rayCast.GetCollisionNormal().Normalized());
			Car_mesh.GlobalTransform = Car_mesh.GlobalTransform.InterpolateWith(xform , turn_speed * 2 * delta);
		}
		

	}
	public Transform Alignwithsurface(Transform xform ,Vector3 new_y)
	{
		xform.basis.y = new_y;
		xform.basis.x = -xform.basis.z.Cross(new_y);
		xform.basis = xform.basis.Orthonormalized();
		return xform;
		
	}
	private void On_collision(Node body)
	{
		
		if(body.IsInGroup("Ramp"))
		{
			Is_on_ramp = true;
		}
		if(body is RigidBody)	
		{
			// Enable_col();
			// get relative velocity
			var col_body = body as RigidBody;
			var relative_velocity = col_body.LinearVelocity - ball.LinearVelocity;
			var Impact_magnitude = relative_velocity.Length();

			// Applying damage
			var damage = Calculate_Damage(Impact_magnitude);
			Apply_Damage(damage , body);
		}
		if(body.IsInGroup("Obstacle") && ball.LinearVelocity.Length() > 6)
		{
			audioStreamPlayer.Play();
			cam.Call("Add_trauma",0.4f);
		}
	}
	public void On_leaving(Node body)
	{
		if(body.IsInGroup("Ramp"))
		{
			Is_on_ramp = false;
		}
	}
	public void Apply_Damage(float damage , Node body)
	{

		health -= damage;
		if(health <= 0)
		{
			Visible = false;
			var e = exp.Instance() as Spatial;
			GetTree().Root.AddChild(e);
			e.GlobalTranslation = ball.GlobalTranslation;
		}
		EmitSignal("Change_Health", health,false);
		body.GetParent().Call("Calculate_Health", damage);
	}
	
	public float Calculate_Damage(float impact_magnitude)
	{
		
		var damage = Mathf.RoundToInt(impact_magnitude * damage_multiplier);
		if(damage <= 3)
		{
			damage = 0;
		}
		else
		{
			audioStreamPlayer.Play();
			cam.Call("Add_trauma",0.5f);
		}
		return damage;
	}
	public void Enable_col()
	{
		col = true;
	}
	public void Disable_col()
	{
		col = false;
	}
	public void Close_miss(RigidBody body)
	{
		if(body.LinearVelocity.Length() > 27)
		{
			health += Close_miss_bonus;
			health = Mathf.Clamp(health , 0 , 100);
			EmitSignal("Change_Health", health,false);
		}
	}

}
