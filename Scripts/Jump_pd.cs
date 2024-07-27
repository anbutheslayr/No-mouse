using Godot;
using System;

public class Jump_pd : Spatial
{
    public AudioStreamPlayer3D Boom;
    public override void _Ready()
    {
        Boom = GetNode<AudioStreamPlayer3D>("Boom");
    }
    public void On_Enter(Node body)
    {
        if(body is RigidBody)
        {
            body.GetParent().Call("Jump");
            Boom.Playing = true;
        }
    }


}
