using Godot;
using System;

public class Explosion : Spatial
{
    public Area area;
    public Camera cam; 
    public override void _Ready()
    {
        area = GetNode<Area>("Area");
        cam = GetTree().Root.GetNode<Camera>("World/Camera");
    }
    public void On_body_entered(Node body)
    {
        if (body is RigidBody)
        {
            var ball = body as RigidBody;
            ball.ApplyImpulse(ball.GlobalTranslation - GlobalTranslation , ((ball.GlobalTranslation - GlobalTranslation).Normalized()+new Vector3(0,0.5f,0))*35);
            cam.Call("Add_trauma",0.9f);
        }
    }
}
