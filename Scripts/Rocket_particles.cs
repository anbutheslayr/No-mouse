using Godot;
using System;

public partial class Rocket_particles : Particles
{
    public Timer timer;
    public override void _Ready()
    {
        timer = new Timer();
        timer.OneShot = true;
        timer.Connect("timeout", new Callable(this, nameof(On_timeout)));
        AddChild(timer);
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public void Kys()
    {
        timer.Start(5);
    }
    public void On_timeout()
    {
        QueueFree();
    }
}
