using Godot;
using System;

public class Enemy_Plane : MeshInstance
{
    public Spatial player;
    public RayCast raycast;
    public Vector3 target_pos;
    [Export] public float FollowAltitude = 10f;
    [Export] public float FollowSpeed = 5f;
    [Export] public float AvoidanceStrength = 10f;
    [Export] public float Rotationspeed = 10f;
    // [Export] public float ObjectDetectionDistance = 10f;
    public override void _Ready()
    {
        player = GetParent().GetNode<Spatial>("taxi/Spatial");
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
       
    }
    public override void _PhysicsProcess(float delta)
    {
        // if (raycast.IsColliding())
        // {
        //     Vector3 avoid_dir = raycast.GetCollisionNormal().Cross(Vector3.Up).Normalized();
        //     target_pos += avoid_dir * AvoidanceStrength;
        // }
        // var direction = (target_pos - GlobalTransform.origin).Normalized();
        // if (direction.Length() > 0.01f)
        // {
        //     var target_forward = direction.Normalized();
        //     var cur_forward = GlobalTransform.basis.z.Normalized();

        //     float angle = Mathf.Acos(cur_forward.Dot(target_forward));
        //     var rotation_axis = cur_forward.Cross(target_forward).Normalized();
        //     var rotation_basis = new Basis(rotation_axis, angle);

        //     GlobalTransform = GlobalTransform.InterpolateWith(GlobalTransform.Rotated(rotation_axis, angle), Rotationspeed * delta);
        // }

        GlobalTranslation = GlobalTranslation.LinearInterpolate(target_pos, FollowSpeed);
    }
}
