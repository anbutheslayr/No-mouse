using Godot;
using System;
using System.Collections;
using System.Diagnostics;

public class Interface : Control
{
    public TouchScreenButton Accelerate_button;
    public TouchScreenButton Brake;
    public TouchScreenButton Left;
    public TouchScreenButton Right;
    public TextureButton Esc;
    public resolution Res;
    public Spatial enemyspawner;
    public PackedScene enemy_taxi;
    public Timer timer;
    public int cur_enemies;
    public Label enemy_spawntext;
    public bool dead = false;
    public bool won = false;
    public float spawn_time;
    public Control PauseMenu;
    public RigidBody plane;
    public RichTextLabel Drift_points;
    public int kill=1;
    public Timer change_world_timer;
    public int cur_world = 1;
    [Export] public int min_kills = 6;
    public override void _Ready()
    {
        plane = GetTree().GetNodesInGroup("Plane")[0] as RigidBody;
        PauseMenu = GetParent().GetParent().GetNode<Pause_menu>("Pause_menu");
        Accelerate_button = GetNode<TouchScreenButton>("Acceleration/Accelerate");
        Brake = GetNode<TouchScreenButton>("Acceleration/Brake");
        Left = GetNode<TouchScreenButton>("Steering/Left");
        Right = GetNode<TouchScreenButton>("Steering/Right");
        Esc = GetNode<TextureButton>("Esc");
        enemyspawner = GetParent().GetParent().GetNode<Spatial>("Enemy_spawner");
        enemy_taxi = GD.Load<PackedScene>("res://Scenes/Enemy_taxi.tscn");
        enemy_spawntext = GetNode<Label>("Enemy_spawntext");
        Verify_res();
        cur_enemies = 0;
        timer = new Timer();
        timer.OneShot = true;
        timer.Connect("timeout", this, nameof(AddEnemies));
        AddChild(timer);
        timer.Start(10);
        change_world_timer = new Timer();
        change_world_timer.OneShot = true;
        change_world_timer.Connect("timeout", this, nameof(ChangeWorld));
        AddChild(change_world_timer);
        spawn_time = 26;
        Drift_points = GetNode<RichTextLabel>("Drift_points");
        Drift_points.Text = "Drift Points: " + Res.Drift_points;

        // Setting shadows
        SetShadow(Res.shadows , Res.ShadowQuality);
        //Setting glow
        SetGlow(Res.Glow);
        
        RepositionAndResize(Res.res);
        SetQuality(Res.Quality);
    }
    public void Verify_res()
    {
        var dir = new Directory();
        dir.Open("user://");
        if(!dir.DirExists("user://Int")) 
        {
            dir.MakeDir("user://Int");
            GD.Print("user://Int created");
        }

        if(dir.FileExists("user://Int/Res.tres"))
        {
            Res = ResourceLoader.Load<resolution>("user://Int/Res.tres");
        }
        else
        {
            Res = ResourceLoader.Load<resolution>("res://Interface/Res.tres");
            GD.Print("Res created");
            ResourceSaver.Save("user://Int/Res.tres", Res);
            Res = ResourceLoader.Load<resolution>("user://Int/Res.tres");
 
        }
    }
    public void SetQuality(int quality)
    {
        switch(quality)
        {
            case 0:
                ProjectSettings.SetSetting("rendering/quality/depth/hdr" , true);
                ProjectSettings.SetSetting("rendering/quality/depth/hdr.mobile" , true);
                ProjectSettings.SaveCustom("res://override.cfg");
                break;
            case 1:
                ProjectSettings.SetSetting("rendering/quality/depth/hdr" , false);
                ProjectSettings.SetSetting("rendering/quality/depth/hdr.mobile" , false);
                ProjectSettings.SaveCustom("res://override.cfg");
                break;
        }
    }
    
    public void AddEnemies()
    {
        if(!dead)
        {
            var enemy = enemy_taxi.Instance() as Spatial;
            enemy.GlobalTransform = enemyspawner.GlobalTransform;
            GetParent().GetParent().AddChild(enemy);
            cur_enemies++;
            if(cur_enemies < Res.NoOfEnemies)
            {
                timer.Start(spawn_time);
                spawn_time -= 2.5f;
            }
            
        }
        
    }
    public void OnEscPressed()
    {
        Engine.TimeScale = 0;
        PauseMenu.Show();
        Hide();
        plane.Call("Pause");
        if(Res.volume != -15)
        {
            var audio_bus = AudioServer.GetBusIndex("Master");
            AudioServer.SetBusMute(audio_bus, true);
        }
        ResourceSaver.Save("user://Int/Res.tres", Res);
    }
    public void SetGlow(bool enabled)
    {
        if(enabled)
        {
            GetTree().Root.GetNode<WorldEnvironment>("World/WorldEnvironment").Environment.GlowEnabled = true;
        }
        else
        {
            GetTree().Root.GetNode<WorldEnvironment>("World/WorldEnvironment").Environment.GlowEnabled = false;
        }
    }
    public void SetShadow(bool enabled , int ShadowQuality)
    {
        if(enabled)
        {
            GetTree().Root.GetNode<DirectionalLight>("World/DirectionalLight").ShadowEnabled = true;
            SetShadowQuality(ShadowQuality);
        }
        else
        {
            GetTree().Root.GetNode<DirectionalLight>("World/DirectionalLight").ShadowEnabled = false;
        }
    }
    public void RepositionAndResize(Vector2 res)
    {
        // Left.Position = new Vector2(229 , -7);
        Left.Position = new Vector2(res.y/1080*316 , res.y/1080*10);
        // Left.Scale = new Vector2(0.668f , 0.677f);
        Left.Scale = new Vector2(res.y/1080*1.198f , res.y/1080*1.204f);
        // Right.Position = new Vector2(286 , -127);
        Right.Position = new Vector2(res.y/1080*340 , res.y/1080*-198);
        Right.Scale = Left.Scale;
        // Accelerate_button.Position = new Vector2(-171 , -215);
        Accelerate_button.Position = new Vector2(res.y/1080*-271 , res.y/1080*-362);
        // Accelerate_button.Scale = new Vector2(0.735f , 0.698f);
        Accelerate_button.Scale = new Vector2(res.y/1080*1.271f , res.y/1080*1.232f);
        // Brake.Position = new Vector2(-383 , -121);
        Brake.Position = new Vector2(res.y/1080*-573 , res.y/1080*-166);
        Brake.Scale = Accelerate_button.Scale;
        Esc.RectScale = new Vector2(res.y/1080, res.y/1080);
        // Esc.RectPosition = new Vector2(96-res.y/1080*96 , 96-res.y/1080*96);
        // Esc.SetPosition(new Vector2(OS.GetScreenSize().x - Esc.RectSize.x , 0));
        enemy_spawntext.MarginTop = res.y/1080*100;
        Drift_points.MarginTop = res.y/1080*10;
        Drift_points.MarginLeft = res.y/1080*-600;
    }
    public void SetShadowQuality(int index)
    {
        switch(index)
        {
            case 0:
                ProjectSettings.SetSetting("rendering/quality/directional_shadow/size" , 2048);
                ProjectSettings.SetSetting("rendering/quality/directional_shadow/size.mobile" , 2048);
                GD.Print("Low Quality");
                break;
            case 1:
                ProjectSettings.SetSetting("rendering/quality/directional_shadow/size" , 4096);
                ProjectSettings.SetSetting("rendering/quality/directional_shadow/size.mobile" , 4096);
                break;
            case 2:
                ProjectSettings.SetSetting("rendering/quality/directional_shadow/size" , 4180);
                ProjectSettings.SetSetting("rendering/quality/directional_shadow/size.mobile" , 4180);
                break;
            case 3:
                ProjectSettings.SetSetting("rendering/quality/directional_shadow/size" , 4864);
                ProjectSettings.SetSetting("rendering/quality/directional_shadow/size.mobile" , 4864);
                break;
        }
    }
    public void Dead()
    {
        dead = true;
    }
    public void AddDriftPoints(float points)
    {
        Res.Drift_points += points*kill;
    }
    public override void _Process(float delta)
    {
        Drift_points.Text = "Drift Points : " + (int)Res.Drift_points;
        enemy_spawntext.Text = "Enemy " + cur_enemies + "/" + Res.NoOfEnemies + " Spawning in " + (int)timer.TimeLeft;
        if(cur_enemies == Res.NoOfEnemies && GetTree().GetNodesInGroup("Enemy").Count !=0)
        {
            enemy_spawntext.Text = "All " + Res.NoOfEnemies+"/"+Res.NoOfEnemies +" enemies spawned";
            if(Res.NoOfEnemies >= min_kills && cur_world < Res.max_worlds)
            {
                change_world_timer.Start(10);    
            }
        }
        if(cur_enemies == Res.NoOfEnemies && GetTree().GetNodesInGroup("Enemy").Count == 0 && !dead && Res.NoOfEnemies >= min_kills)
        {
            if(cur_world < Res.max_worlds)
            {
                enemy_spawntext.Text = "\n\n Teleporting to next world in " + (int)change_world_timer.TimeLeft;
            }
            won = true;
        }
        else if(cur_enemies == Res.NoOfEnemies && GetTree().GetNodesInGroup("Enemy").Count == 0 && !dead && Res.NoOfEnemies < min_kills)
        {
            enemy_spawntext.Text = "\n\n\nAtleast defeat " + min_kills + " enemies to get to next world \n You can change the number of enemies in settings";
            won = true;
        }
        if(dead && !won)
        {
            // enemy_spawntext.Text = "You Lost :( \n To " + cur_enemies + " Enemies \n Try Again";
            // if(Res.volume != -15)
            // {
            //     var audio_bus = AudioServer.GetBusIndex("Master");
            //     AudioServer.SetBusMute(audio_bus, true);
            // }
            Hide();
            GetParent().GetParent().GetNode<Control>("Death_screen").Show();
            GetParent().GetParent().GetNode<Control>("Death_screen").Call("Start");
            plane.Call("Pause");

            
        }
        if(Input.IsActionJustPressed("ui_cancel"))
        {
            OnEscPressed();
        }
    }
    public void Kill()
    {
        kill+=1;
    }
    public void ChangeWorld()
    {
        cur_world++;
        GetTree().ChangeScene("res://Scenes/Worlds/World"+cur_world+".tscn");
    }
}

