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
    public float spawn_time;
    public override void _Ready()
    {
        Accelerate_button = GetNode<TouchScreenButton>("Acceleration/Accelerate");
        Brake = GetNode<TouchScreenButton>("Acceleration/Brake");
        Left = GetNode<TouchScreenButton>("Steering/Left");
        Right = GetNode<TouchScreenButton>("Steering/Right");
        Esc = GetNode<TextureButton>("Esc");
        enemyspawner = GetParent().GetParent().GetNode<Spatial>("Enemy_spawner");
        enemy_taxi = GD.Load<PackedScene>("res://Scenes/Enemy_taxi.tscn");
        enemy_spawntext = GetNode<Label>("Enemy_spawntext");
        Res = GD.Load<resolution>("res://Interface/Res.tres");
        cur_enemies = 0;
        timer = new Timer();
        timer.OneShot = true;
        timer.Connect("timeout", this, nameof(AddEnemies));
        AddChild(timer);
        timer.Start(10);
        spawn_time = 26;

        // Setting shadows
        SetShadow(Res.shadows , Res.ShadowQuality);
        //Setting glow
        SetGlow(Res.Glow);
        
        RepositionAndResize(Res.res);
    }
    
    public void AddEnemies()
    {
        if(dead == false)
        {
            var enemy = enemy_taxi.Instance() as Spatial;
            enemy.GlobalTransform = enemyspawner.GlobalTransform;
            GetParent().GetParent().AddChild(enemy);
            cur_enemies++;
            if(cur_enemies < Res.NoOfEnemies)
            {
                GD.Print(spawn_time);
                timer.Start(spawn_time);
                spawn_time -= 2.5f;
            }
            else
            {
                enemy_spawntext.Text = "All enemies spawned";
            }
        }
        
    }
    public void OnEscPressed()
    {
        GetTree().ChangeScene("res://Scenes/Main_menu.tscn");
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
    public override void _Process(float delta)
    {
        enemy_spawntext.Text = "Enemy " + cur_enemies + "/" + Res.NoOfEnemies + " Spawning in " + (int)timer.TimeLeft;
        if(cur_enemies == Res.NoOfEnemies)
        {
            enemy_spawntext.Text = "All " + Res.NoOfEnemies+"/"+Res.NoOfEnemies +" enemies spawned";
        }
        if(cur_enemies == Res.NoOfEnemies && GetTree().GetNodesInGroup("Enemy").Count == 0)
        {
            enemy_spawntext.Text = "You Won :) \n Against " + cur_enemies + " Enemies";
        }
        if(dead)
        {
            enemy_spawntext.Text = "You Lost :( \n To " + cur_enemies + " Enemies \n Try Again";
        }
    }
}

