using Godot;
using System;

public class Rocket_Ammo : Area
{
    public bool launch=false;
    public Vector3 direction;
    public Spatial marker;
    public Spatial targ;
    public Timer timer;
    public PackedScene Explosion;
    public PackedScene part;
    public Spatial partpos;
    public Particles p;
    public bool can_turn = false;
    [Export] public float rot_speed = .2f;
    public AudioStreamPlayer3D thrust;
    public override void _Ready()
    {
        marker = GetNode<Spatial>("target");
        Explosion = GD.Load<PackedScene>("res://Scenes/Explosion.tscn");
        part = GD.Load<PackedScene>("res://Scenes/Rocket particles.tscn");
        partpos = GetNode<Spatial>("part");
        thrust = GetNode<AudioStreamPlayer3D>("Thruster");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _PhysicsProcess(float delta)
    {
        if(launch)
        {
            p.GlobalTransform = partpos.GlobalTransform;    
            GlobalTranslate(GlobalTransform.basis.z * delta*100);
            targ = GetClosestEnemy();
            if(targ != null)
            {
                // GD.Print("turning");
                direction = (targ.GlobalTranslation - GlobalTranslation).Normalized();
                LookAt(marker.GlobalTranslation.LinearInterpolate(GlobalTransform.origin - direction , rot_speed*delta),Vector3.Up);
            }
            

        }
        
    }
    public Spatial GetClosestEnemy()
    {
        Spatial closest_enemy = null;
        float closest_distance = 0;
        foreach(Spatial enemy in GetTree().GetNodesInGroup("Enemy") + GetTree().GetNodesInGroup("Runnable"))
        {
            float distance = GlobalTransform.origin.DistanceTo(enemy.GlobalTransform.origin);
            if(closest_enemy == null || distance < closest_distance)
            {
                closest_enemy = enemy;
                closest_distance = distance;
            }
        }
        return closest_enemy;
    }
    public void Launch()
    {
        launch = true;
        p = part.Instance() as Particles;
        GetTree().Root.AddChild(p);
        p.GlobalTranslation = partpos.GlobalTranslation;    
        p.Emitting = true;
        thrust.Playing = true;
        SetAsToplevel(true);

    }
    public void On_collision(Node node)
    {
        if (node is RigidBody && node.IsInGroup("Enemy") && launch)
        {
            var e = Explosion.Instance() as Spatial;
            GetTree().Root.AddChild(e);
            e.GlobalTranslation = GlobalTranslation;
            node.GetParent().Call("Calculate_Health", 20);
            QueueFree();
            p.Emitting = false;
            p.Call("Kys");
        }
        else if (node.IsInGroup("Runnable") && launch)
        {
            var e = Explosion.Instance() as Spatial;
            GetTree().Root.AddChild(e);
            e.GlobalTranslation = GlobalTranslation;
            node.GetParent().GetParent().GetParent().Call("Calculate_Health");
            node.GetParent().GetParent().GetParent().Call("Calculate_Health");
            node.GetParent().GetParent().GetParent().Call("Calculate_Health");
            node.GetParent().GetParent().GetParent().Call("Calculate_Health");
            node.GetParent().GetParent().GetParent().Call("Calculate_Health");
            QueueFree();
            p.Emitting = false;
            p.Call("Kys");
        }
        else if (node.IsInGroup("Ground") && launch)
        {
            var e = Explosion.Instance() as Spatial;
            GetTree().Root.AddChild(e);
            e.GlobalTranslation = GlobalTranslation;
            QueueFree();
            p.Emitting = false;
            p.Call("Kys");
        }
    }

}
