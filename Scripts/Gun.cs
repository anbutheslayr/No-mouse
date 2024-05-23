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
    [Export] public string Enemy_healthbar;
    [Export] public int gun_damage = 5;
    [Export] public float Aim_speed = 5f;
    [Export] public int Range = 3;
    public Spatial Marker;
    public AudioStreamPlayer audioStreamPlayer;
    public override void _Ready()
    {
        ray_cast = GetNode<RayCast>(RayCastPath);
        ray_cast.Enabled = true;
        anim = GetParent().GetNode<AnimationPlayer>(AnimPath);
        gun = GetNode<MeshInstance>(gun_path);
        decal = GD.Load<PackedScene>(decal_path);
        random = new Random();
        audioStreamPlayer = GetNode<AudioStreamPlayer>("AudioStreamPlayer");
        Marker = GetNode<Spatial>("Gun/Marker");
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
            var direction = closest_enemy.GlobalTransform.origin - GlobalTransform.origin;
            
            if( gun.GlobalTransform.origin.DistanceTo(closest_enemy.GlobalTransform.origin) > Range)
            {
                gun.LookAt(Marker.GlobalTransform.origin.LinearInterpolate(GlobalTransform.origin - direction , Aim_speed * delta),Vector3.Up);
                if(anim.CurrentAnimation != "Shoot")
                {
                    anim.Play("Shoot");
                }
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
    private void OnShoot()
    {
        if(ray_cast.IsColliding())
        {
            var b = decal.Instance() as Spatial;
            (ray_cast.GetCollider() as Node).AddChild(b);
            var transform = b.GlobalTransform;
            transform.origin = ray_cast.GetCollisionPoint();
            b.GlobalTransform = transform;
            if(ray_cast.GetCollisionNormal() != Vector3.Up)
            {
                b.LookAt(ray_cast.GetCollisionPoint() + ray_cast.GetCollisionNormal(), Vector3.Up);
            }
            if( (ray_cast.GetCollider() as Node).IsInGroup("Enemy_Body"))
            {
                var enemy =(ray_cast.GetCollider() as Node).GetParent().GetParent().GetParent() as Spatial;
                enemy.Call("Calculate_Health" , gun_damage);
                // GD.Print("damage = " + gun_damage);
            }
        }
        

    }
}
