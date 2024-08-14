using Godot;
using System;

public class Col_jmpd : Spatial
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
