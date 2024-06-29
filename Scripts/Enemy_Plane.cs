using Godot;
using System;

public class Enemy_Plane : RigidBody
{
	public Spatial player;
	public RayCast raycast;
	public Vector3 target_pos;

	public MeshInstance capsule;
	[Export] public float FollowAltitude = 10f;
	[Export] public float FollowSpeed = 5f;
	[Export] public float AvoidanceStrength = 10f;
	// [Export] public float ObjectDetectionDistance = 10f;
	public override void _Ready()
	{
		player = GetParent().GetNode<Spatial>("taxi/Spatial");
		capsule = GetNode<MeshInstance>("MeshInstance");
		raycast = new RayCast();
		AddChild(raycast);
		raycast.Enabled = true;
		raycast.AddException(this);
		raycast.AddException(player);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		target_pos = player.GlobalTransform.origin + new Vector3(0, FollowAltitude, 0);
		raycast.CastTo = target_pos - GlobalTransform.origin;
		raycast.ForceRaycastUpdate();
		if (GlobalTranslation.DistanceTo(player.GlobalTranslation) < 4)
		{
			player.GetParent().Call("Enable_col");
			capsule.Show();
		}
		else
		{
			player.GetParent().Call("Disable_col");
			capsule.Hide();
		}
	   
	}
	public override void _PhysicsProcess(float delta)
	{
		if (raycast.IsColliding())
		{
			Vector3 avoid_dir = raycast.GetCollisionNormal().Cross(Vector3.Up).Normalized();
			target_pos += avoid_dir * AvoidanceStrength;
		}
		
		GlobalTranslation = GlobalTranslation.LinearInterpolate(target_pos, FollowSpeed);
	}
	
}
