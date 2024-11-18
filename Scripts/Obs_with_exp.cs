using Godot;
using System;

public class Obs_with_exp : StaticBody
{
    public PackedScene exp;
    public override void _Ready()
    {
        exp = GD.Load<PackedScene>("res://Scenes/Explosion.tscn");
    }

    public void On_col(Node body)
    {
        if(body is RigidBody)
        {
            var e = exp.Instance() as Spatial;
            GetTree().Root.AddChild(e);
            e.GlobalTranslation = GlobalTranslation; 
        }

    }
}
