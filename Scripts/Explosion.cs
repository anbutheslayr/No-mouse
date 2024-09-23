using Godot;
using System;

public class Explosion : Spatial
{
    public Particles Debris;
    public Particles Smoke;
    public Particles fire;
    public Timer timer;
    public AudioStreamPlayer3D Boom;
    
    public override void _Ready()
    {
        Debris = GetNode<Particles>("Debris");
        Smoke = GetNode<Particles>("Smoke");
        fire = GetNode<Particles>("fire");
        timer = new Timer();
        timer.OneShot = true;
        Boom = GetNode<AudioStreamPlayer3D>("Boom");
    }

    public void Explode()
    {
        Debris.Emitting = true;
        Smoke.Emitting = true;
        fire.Emitting = true;
        Boom.Playing = true;
        AddChild(timer);
        timer.Start(2f);
        timer.Connect("timeout", this, nameof(qf));
    }

    public void qf()
    {
        QueueFree();
    }
}
