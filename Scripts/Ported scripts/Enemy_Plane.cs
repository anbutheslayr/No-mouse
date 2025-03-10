using Godot;
using System;

public partial class Enemy_Plane : RigidBody3D
{
	public RigidBody3D player;
	public RayCast3D raycast;
	public Vector3 target_pos;

	public MeshInstance3D capsule;
	[Export] public float FollowAltitude = 10f;
	[Export] public float FollowSpeed = 5f;
	[Export] public float AvoidanceStrength = 10f;
	[Export] public bool paused = false;
	public AudioStreamPlayer Attack;
	public Camera3D Camera3D;
	[Export] public float trauma_amount = 0.5f;
	// [Export] public float ObjectDetectionDistance = 10f;
	public override void _Ready()
	{
		player = GetParent().GetNode<RigidBody3D>("taxi/Ball");
		capsule = GetNode<MeshInstance3D>("MeshInstance3D");
		Attack = GetNode<AudioStreamPlayer>("Attack");
		raycast = new RayCast3D();
		AddChild(raycast);
		raycast.Enabled = true;
		raycast.AddException(this);
		raycast.AddException(player);
		Camera3D = GetParent().GetNode<Camera3D>("Camera3D");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		
		target_pos = player.GlobalTransform.origin + new Vector3(0, FollowAltitude, 0);
		raycast.TargetPosition = target_pos - GlobalTransform.origin;
		raycast.ForceRaycastUpdate();
		if (GlobalTranslation.DistanceTo(player.GlobalTranslation) < 4)
		{
			player.GetParent().Call("Enable_col");
			capsule.Show();
			Attack.Playing = true;
			Camera3D.Call("Add_trauma" , trauma_amount);
		}
		else
		{
			player.GetParent().Call("Disable_col");
			capsule.Hide();
			Attack.Playing = false;
		}

		

	}
	public override void _PhysicsProcess(float delta)
	{
		
		
		if (raycast.IsColliding())
		{
			Vector3 avoid_dir = raycast.GetCollisionNormal().Cross(Vector3.Up).Normalized();
			target_pos += avoid_dir * AvoidanceStrength;
		}
		if(!paused)
		{
			GlobalTranslation = GlobalTranslation.Lerp(target_pos, FollowSpeed);
		}
		

	}
	public void Pause()
	{
		paused = true;
	}
	public void Resume()
	{
		paused = false;
	}
}
