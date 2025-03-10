using Godot;
using System;

public partial class Obs_with_exp : StaticBody3D
{
    public PackedScene exp;
    public override void _Ready()
    {
        exp = GD.Load<PackedScene>("res://Scenes/Explosion.tscn");
    }

    public void On_col(Node body)
    {
        if(body is RigidBody3D)
        {
            var e = exp.Instance() as Node3D;
            GetTree().Root.AddChild(e);
            e.GlobalTranslation = GlobalTranslation; 
        }
    }
}
