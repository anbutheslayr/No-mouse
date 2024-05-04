using Godot;
using System;


public class Movement : Spatial
{
	public RigidBody ball;
	public MeshInstance Car_mesh;
	public RayCast rayCast;
	[Export] public string Ball_path;
	[Export] public string Car_mesh_path;
	[Export] public string rayCast_path; 
	[Export] public Vector3 sphere_offset = new Vector3(0, 7, 6);
	[Export] public float acceleration = 50;
	[Export] public float steering = 21;
	[Export] public float turn_speed = 5;
	[Export] public float turn_stop_limit = 0.75f;
	public float speed_input;
	public float steering_input;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ball = GetNode<RigidBody>(Ball_path);
		Car_mesh = GetNode<MeshInstance>(Car_mesh_path);
		rayCast = GetNode<RayCast>(rayCast_path);
		rayCast.AddException(ball);
	}

	public override void _PhysicsProcess(float delta)
	{
		// align te mesh with sphere
		var transform = ball.Transform;
		transform.origin = ball.Transform.origin + sphere_offset;
		Car_mesh.Transform = transform;
		//Accelerate
		ball.AddCentralForce(-transform.basis.z * speed_input);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		// Acceleration
		speed_input = Input.GetActionStrength("ui_up") - Input.GetActionStrength("ui_down");
		speed_input *= acceleration;
		// Steering
		steering_input = Input.GetActionStrength("ui_right") - Input.GetActionStrength("ui_left");
		steering_input = Mathf.Deg2Rad(steering);
		// Apply steering
		if(ball.LinearVelocity.Length() > turn_stop_limit)
		{
			var new_basis = Car_mesh.GlobalTransform.basis.Rotated(Car_mesh.GlobalTransform.basis.y ,steering_input );
			var transform = Car_mesh.GlobalTransform;
			transform.basis = Car_mesh.GlobalTransform.basis.Slerp(new_basis, turn_speed * delta);
			Car_mesh.GlobalTransform = transform.Orthonormalized();
		}

	}
}
