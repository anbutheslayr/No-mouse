using Godot;
using System;

public class ghost : RigidBody
{
    public RigidBody player;
    public RayCast raycast;
    public Vector3 target_pos;

    [Export] public float FollowAltitude = 10f;
    [Export] public float FollowSpeed = 5f;
    [Export] public float AvoidanceStrength = 10f;
    [Export] public bool paused = false;
    public AudioStreamPlayer Attack;
    public Camera Camera;
    [Export] public float trauma_amount = 0.5f;    

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {

          player = GetParent().GetNode<RigidBody>("taxi/Ball");
          Attack = GetNode<AudioStreamPlayer>("Attack");
          raycast = new RayCast();
          AddChild(raycast);
          raycast.Enabled = true;
          raycast.AddException(this);
          raycast.AddException(player);
          Camera = GetParent().GetNode<Camera>("Camera");        
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
          Attack.Playing = true;
          Camera.Call("Add_trauma" , trauma_amount);
        }
        else
        {
          player.GetParent().Call("Disable_col");
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
        GlobalTranslation = GlobalTranslation.LinearInterpolate(target_pos, FollowSpeed);
        LookAt(player.GlobalTranslation, Vector3.Up);
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
