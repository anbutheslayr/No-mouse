using Godot;
using System;

public class Enemy_Plane : RigidBody
{
    public Spatial player;
    public RayCast raycast;
    public Vector3 target_pos;
    [Export] public float FollowAltitude = 10f;
    [Export] public float FollowSpeed = 5f;
    public Spatial Marker;
    public override void _Ready()
    {
        player = GetParent().GetNode<Spatial>("taxi/Spatial");
        Marker = GetNode<Spatial>("Marker");
        raycast = new RayCast();
        AddChild(raycast);
        raycast.Enabled = true;
        raycast.AddException(this);
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        target_pos = player.GlobalTransform.origin + new Vector3(0, FollowAltitude, 0);
        var direction = target_pos - GlobalTransform.origin;
        LookAt(Marker.GlobalTransform.origin.LinearInterpolate(GlobalTransform.origin - direction , FollowSpeed),Vector3.Up);
        AddForce(direction * FollowSpeed ,-GlobalTransform.basis.z );
    }
}
