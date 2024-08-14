using Godot;
using System;

public class Jump_pd : Spatial
{
    public AudioStreamPlayer3D Boom;
    public RigidBody Body;
    public bool Entered=false;
    public override void _Ready()
    {
        Boom = GetNode<AudioStreamPlayer3D>("Boom");
    }
    public void On_Enter(Node body)
    {
        if(body is RigidBody)
        {
            Entered = true;
            Body = body as RigidBody;
        }
    }
    public void On_Exit(Node body)
    {
        if(body is RigidBody)
        {
            Entered = false;
        }
    }
    public override void _PhysicsProcess(float delta)
    {
        if(Entered)
        {
            Body.GetParent().Call("Jump");
            Boom.Playing = true;
        }
    }


}
