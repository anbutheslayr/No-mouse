using Godot;
using System;
using System.Collections.Generic;

public class Gun : Spatial
{
    [Export] public string RayCastPath;
    [Export] public string AnimPath;
    public MeshInstance gun;
    [Export] public string gun_path;
    public AnimationPlayer anim;
    public RayCast ray_cast;
    public List<Spatial> enemies = new List<Spatial>();
    public PackedScene decal;
    [Export] public string decal_path;
    public Random random;
    [Export] public float offsetRange = 1f;
    public override void _Ready()
    {
        ray_cast = GetNode<RayCast>(RayCastPath);
        ray_cast.Enabled = true;
        anim = GetParent().GetNode<AnimationPlayer>(AnimPath);
        gun = GetNode<MeshInstance>(gun_path);
        decal = GD.Load<PackedScene>(decal_path);
        random = new Random();
    }

    public void OnDetection(Node body)
    {
        if(body.IsInGroup("Enemy"))
        {
            enemies.Add(body as Spatial);
            GD.Print(enemies);
        }
    }
    public void OnExit(Node body)
    {
        if(body.IsInGroup("Enemy"))
        {
            enemies.Remove(body as Spatial);
        }
    }
    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        if(enemies.Count > 0 && GetClosestEnemy() != null)
        {
            Spatial closest_enemy = GetClosestEnemy();
            
                if(anim.CurrentAnimation != "Shoot")
                {
                    gun.LookAt(AimAt(closest_enemy.GlobalTransform.origin), Vector3.Up);
                    anim.Play("Shoot");
                }
        }
    }
    public Spatial GetClosestEnemy()
    {
        Spatial closest_enemy = null;
        float closest_distance = 0;
        foreach(Spatial enemy in enemies)
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
    private Vector3 AimAt(Vector3 target)
    {
        Vector3 offset = new Vector3
        ((float)(random.NextDouble() * 2 - 1) * offsetRange,
        (float)(random.NextDouble() * 2 - 1) * offsetRange,
        (float)(random.NextDouble() * 2 - 1) * offsetRange);

        Vector3 direction = target - GlobalTransform.origin;
        
        return GlobalTransform.origin - (direction+offset); ;
    }
    private void OnShoot()
    {
        if( ray_cast.IsColliding() )
        {
            GD.Print("Shot");
            GD.Print(ray_cast.GetCollider());
            var b = decal.Instance() as Spatial;
            (ray_cast.GetCollider() as Node).AddChild(b);
            var transform = b.GlobalTransform;
            transform.origin = ray_cast.GetCollisionPoint();
            b.GlobalTransform = transform;
            b.LookAt(ray_cast.GetCollisionPoint() + ray_cast.GetCollisionNormal(), Vector3.Up);
        }

    }
}
