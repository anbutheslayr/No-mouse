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
	[Export] public Vector3 sphere_offset = new Vector3(0, -1, 0);
	[Export] public float acceleration = 50;
	[Export] public float steering = 50;
	[Export] public float turn_speed = 5;
	[Export] public float turn_stop_limit = 0.75f;
	public float speed_input;
	public float steering_input;
	[Export] public string left_wheel_path;
	[Export] public string right_wheel_path;
	public MeshInstance left_wheel;
	public MeshInstance right_wheel;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ball = GetNode<RigidBody>(Ball_path);
		Car_mesh = GetNode<MeshInstance>(Car_mesh_path);
		rayCast = GetNode<RayCast>(rayCast_path);
		rayCast.AddException(ball);
		left_wheel = GetNode<MeshInstance>(left_wheel_path);
		right_wheel = GetNode<MeshInstance>(right_wheel_path);
	}

	public override void _PhysicsProcess(float delta)
	{
		// align te mesh with sphere
		var transform = Car_mesh.Transform;
		transform.origin = ball.Transform.origin + sphere_offset;
		Car_mesh.Transform = transform;
		//Accelerate
		ball.AddCentralForce(-Car_mesh.GlobalTransform.basis.z * speed_input);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		// Acceleration
		speed_input = 0;
		speed_input += Input.GetActionStrength("ui_up");
		speed_input -=  Input.GetActionStrength("ui_down");
		speed_input *= acceleration;
		//Steering 
		steering_input = 0;
		steering_input -= Input.GetActionStrength("ui_right");
		steering_input += Input.GetActionStrength("ui_left");
		steering_input *= Mathf.Deg2Rad(steering);
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
			var new_basis = Car_mesh.GlobalTransform.basis.Rotated(Car_mesh.GlobalTransform.basis.y ,steering_input );
			var transform = Car_mesh.GlobalTransform;
			transform.basis = Car_mesh.GlobalTransform.basis.Slerp(new_basis, turn_speed * delta);
			Car_mesh.GlobalTransform = transform.Orthonormalized();
		}
		// Align with surface
		var n = rayCast.GetCollisionNormal().Normalized();
		var xform = Alignwithsurface(Car_mesh.GlobalTransform ,n);
		Car_mesh.GlobalTransform = Car_mesh.GlobalTransform.InterpolateWith(xform , 0.2f);

	}
	public Transform Alignwithsurface(Transform xform ,Vector3 new_y)
	{
		xform.basis.y = new_y;
		xform.basis.x = xform.basis.z.Cross(new_y);
		xform.basis = xform.basis.Orthonormalized();
		return xform;
	}
}
