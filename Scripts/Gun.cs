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
    public PackedScene Particles;
    [Export] public string decal_path;
    public Random random;
    [Export] public int gun_damage = 5;
    [Export] public float Aim_speed = .25f;
    [Export] public float Range = 2;
    public Spatial Marker;
    public AudioStreamPlayer audioStreamPlayer;
    public bool entered = false;
    [Export] public int Ammo;
    [Export] public int start_magazines = 1;
    public int cur_magazines;
    public int cur_ammo;
    public RichTextLabel ammo_text;
    public Spatial player;
    public override void _Ready()
    {
        ray_cast = GetNode<RayCast>(RayCastPath);
        ray_cast.Enabled = true;
        anim = GetParent().GetParent().GetNode<AnimationPlayer>(AnimPath);
        gun = GetNode<MeshInstance>(gun_path);
        decal = GD.Load<PackedScene>(decal_path);
        random = new Random();
        audioStreamPlayer = GetNode<AudioStreamPlayer>("AudioStreamPlayer");
        Marker = GetNode<Spatial>("Gun/Marker");
        ammo_text = GetParent().GetParent().GetParent().GetNode<RichTextLabel>("Interface/Ammo_text");
        cur_ammo = Ammo;
        cur_magazines = start_magazines;
        ProjectSettings.SetSetting("display/window/stretch/mode" , "disabled");
        Particles = GD.Load<PackedScene>("res://Scenes/Particles.tscn");
        player = GetParent().GetParent().GetParent() as Spatial;
    }

    public void OnDetection(Node body)
    {
        if(body.IsInGroup("Enemy"))
        {
            enemies.Add(body as Spatial);
            // GD.Print(enemies);
        }
    }
    public void OnExit(Node body)
    {
        if(body.IsInGroup("Enemy"))
        {
            enemies.Remove(body as Spatial);
            player.Call("Close_miss" , body);
        }
    }
    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        ammo_text.Text = "       Ammo = " + cur_ammo + "/" + Ammo + "(" + cur_magazines + ") \n       " + OS.GetScreenSize().ToString() + "\n       FPS : " + Engine.GetFramesPerSecond() + "\n       Enemies Alive : " + GetTree().GetNodesInGroup("Enemy").Count;
        if(anim.CurrentAnimation == "Reload")
        {
            ammo_text.Text = "Reloading...";
        }
        if(enemies.Count > 0 && GetClosestEnemy() != null && cur_ammo > 0 && anim.CurrentAnimation != "Reload")
        {
            if(entered == false && anim.CurrentAnimation != "Gun_descend")
            {
                anim.Play("Gun_rise");
                entered = true;
            }
            Spatial closest_enemy = GetClosestEnemy();
            var direction = closest_enemy.GlobalTransform.origin - GlobalTransform.origin;
            
            if( gun.GlobalTransform.origin.DistanceTo(closest_enemy.GlobalTransform.origin) > Range )
            {
                gun.LookAt(Marker.GlobalTransform.origin.LinearInterpolate(GlobalTransform.origin - direction , Aim_speed),Vector3.Up);
                if(anim.CurrentAnimation != "Shoot" && anim.CurrentAnimation != "Gun_rise" && anim.CurrentAnimation != "Gun_descend")
                {
                    anim.Play("Shoot");
                }
            }

                
        }
        else if(cur_ammo <= 0 && cur_magazines > 0)
        {
            anim.Play("Reload");
            cur_ammo = Ammo;
            cur_magazines--;
        }
        else if(entered == true && anim.CurrentAnimation != "Gun_rise" && anim.CurrentAnimation != "Reload")
        {
            anim.Play("Gun_descend");
            entered = false;
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
        
        if(ray_cast.IsColliding() && cur_ammo > 0)
        {
            var a = Particles.Instance() as Spatial;
            var b = decal.Instance() as Spatial;
            GetTree().Root.AddChild(a);
            (ray_cast.GetCollider() as Node).AddChild(b);
            a.GlobalTranslation = ray_cast.GetCollisionPoint();
            b.GlobalTranslation = ray_cast.GetCollisionPoint();
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
        else if(cur_ammo <= 0)
        {
            cur_ammo = 1;
        }
        cur_ammo--;
    }
    public void Add_ammo()
    {
        cur_magazines++;
    }
}
