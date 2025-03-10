using Godot;
using System;

public partial class Col_jmpd : Node3D
{
    public AudioStreamPlayer3D Boom;
    public override void _Ready()
    {
        Boom = GetNode<AudioStreamPlayer3D>("Boom");
    }
    public void boom()
    {
        Boom.Play();
    }
    


}
