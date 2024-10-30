using Godot;
using System;

public class Explosion : Spatial
{
    public Area area;
    public override void _Ready()
    {
        area = GetNode<Area>("Area");
    }
    public void On_body_entered(Node body)
    {
        if (body is RigidBody)
        {
            (body as RigidBody).ApplyImpulse(GlobalTranslation - (body as RigidBody).GlobalTranslation , ((GlobalTranslation-(body as RigidBody).GlobalTranslation).Normalized()+new Vector3(0,1.2f,0))*20);
        }
    }
}
