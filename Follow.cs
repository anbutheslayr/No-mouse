using Godot;
using System;

public class Follow : Camera
{
    [Export] public float lerp_speed = 3;
    [Export] public string target_PATH;
    [Export] public Vector3 offset;
    public Rigidbody target;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        target = GetNode<Rigidbody>(target_PATH);
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        var target_pos = target
    }
}
