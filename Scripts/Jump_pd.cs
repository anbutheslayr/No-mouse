using Godot;
using System;

public partial class Jump_pd : Node3D
{
    public AudioStreamPlayer3D Boom;
    public RigidBody3D Body;
    public bool Entered=false;
    public override void _Ready()
    {
        Boom = GetNode<AudioStreamPlayer3D>("Boom");
    }
    public void On_Enter(Node body)
    {
        if(body is RigidBody3D)
        {
            Entered = true;
            Body = body as RigidBody3D;
        }
    }
    public void On_Exit(Node body)
    {
        if(body is RigidBody3D)
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
