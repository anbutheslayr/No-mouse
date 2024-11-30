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
    public int cur_gun = 0;
    public Spatial rocket_launcher;
    public Timer rock_timer;
    public bool launch = false;
    public int i = 0;
    public PackedScene roc_ammo;
    [Export] public int rocket_ammo = 15;
    public int max_bull_ind=2;
    public TextureButton rocket_button;
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
        rocket_launcher = GetParent().GetNode<Spatial>("Rocket Launcher/Gun");
        rock_timer = new Timer();
        AddChild(rock_timer);
        rock_timer.OneShot = true;
        rock_timer.WaitTime = .2f;
        rock_timer.Connect("timeout" , this , nameof(On_timeout));
        rock_timer.Start();
        roc_ammo = GD.Load<PackedScene>("res://Assets/Models/Guns/Rocket Ammo.tscn");
        rocket_button = GetParent().GetParent().GetParent().GetNode<TextureButton>("Interface/Rocket_button");
    }

    public void OnDetection(Node body)
    {
        if(body.IsInGroup("Enemy"))
        {
            enemies.Add(body as Spatial);
            // GD.Print(enemies);
        }
        if(body.IsInGroup("Runnable"))
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
        if(body.IsInGroup("Runnable"))
        {
            enemies.Remove(body as Spatial);
            player.Call("Close_miss" , body);
        }
    }
    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        Spatial closest_enemy = GetClosestEnemy();
        var direction = Vector3.Zero;
        ammo_text.Text = "       Ammo = " + cur_ammo + "/" + Ammo + "(" + cur_magazines + ") \n       " + OS.GetScreenSize().ToString() + "\n       FPS : " + Engine.GetFramesPerSecond() + "\n       Enemies Alive : " + GetTree().GetNodesInGroup("Enemy").Count;

        if(closest_enemy != null)
            direction = closest_enemy.GlobalTransform.origin - GlobalTransform.origin;
        switch(cur_gun)
        {
            case 0:
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
                   
                
                    if( gun.GlobalTransform.origin.DistanceTo(closest_enemy.GlobalTransform.origin) > Range )
                    {
                        var aimspd = Aim_speed;
                        if(GetClosestEnemy().IsInGroup("Runnable"))
                        {
                            aimspd = 0.95f;
                        }
                        gun.LookAt(Marker.GlobalTransform.origin.LinearInterpolate(GlobalTransform.origin - direction , aimspd),Vector3.Up);
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
                else if(entered == true && !anim.IsPlaying())
                {
                    anim.Play("Gun_descend");
                    entered = false;
                }
                break;




                case 1:

                ammo_text.Text = "       Ammo = " + rocket_ammo*3 + "/" + 45 + "(" + rocket_ammo + ") \n       " + OS.GetScreenSize().ToString() + "\n       FPS : " + Engine.GetFramesPerSecond() + "\n       Enemies Alive : " + GetTree().GetNodesInGroup("Enemy").Count;

                if(rocket_ammo > 0 && rocket_launcher.GetChildCount() > 0)
                {
                    if(entered == false && !anim.IsPlaying() && GetTree().GetNodesInGroup("Enemy").Count > 0)
                    {
                       anim.Play("Rocket_rise");
                       entered = true;
                    }
                    if(entered == true && !anim.IsPlaying())
                    {
                        if(launch)
                        {
                            rocket_launcher.GetChild(rocket_launcher.GetChildCount() - 1).Call("Launch");
                            Reparent(rocket_launcher.GetChild(rocket_launcher.GetChildCount() - 1) as Spatial);
                            launch = false;
                            rock_timer.Start(2);
                            i++;
                        }
                        
                        
                    }
                    
                }
                else if(enemies.Count > 0 && GetClosestEnemy() != null && rocket_ammo > 0 && rocket_launcher.GetChildCount() == 0)
                {
                    anim.Play("Rocket_reload");
                    entered = false;
                }
                else if(entered == true && !anim.IsPlaying())
                {
                    anim.PlayBackwards("Rocket_rise");
                    entered = false;
                }
                
                
                break;
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
    public void On_timeout()
    {
        launch = true;
    }
    public void Rocket_reload()
    {
        var b1 = roc_ammo.Instance() as Spatial;
        rocket_launcher.AddChild(b1);
        b1.GlobalTransform = (rocket_launcher.GetParent().GetChild(0)as Spatial).GlobalTransform;
        var b2 = roc_ammo.Instance() as Spatial;
        rocket_launcher.AddChild(b2);
        b2.GlobalTransform = (rocket_launcher.GetParent().GetChild(1)as Spatial).GlobalTransform;
        var b3 = roc_ammo.Instance() as Spatial;
        rocket_launcher.AddChild(b3);
        b3.GlobalTransform = (rocket_launcher.GetParent().GetChild(2)as Spatial).GlobalTransform;
        rocket_ammo--;
        gun_switch();
    }
    public void Reparent(Spatial bullet)
    {
        var transform = bullet.GlobalTransform;
        var oldparent = bullet.GetParent();
        oldparent.RemoveChild(bullet);
        GetTree().Root.GetChild(0).AddChild(bullet);
        bullet.GlobalTransform = transform;
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
            if( (ray_cast.GetCollider() as Node).IsInGroup("Runnable"))
            {
                (ray_cast.GetCollider() as Node).GetParent().GetParent().GetParent().Call("Calculate_Health");
            }
            audioStreamPlayer.Play();
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
    public void gun_switch()
    {
        if(cur_gun == 1)
        {
            cur_gun = 0;
            rocket_button.Disabled = false;
        }
        else
        {
            cur_gun++;
            rocket_button.Disabled = true;
            if(anim.IsPlaying() && anim.CurrentAnimation != "Gun_descend")
            {
                anim.Play("Gun_descend");
                entered = false;
            }
        }
    }
}
