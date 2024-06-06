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
    public override void _Ready()
    {
        Accelerate_button = GetNode<TouchScreenButton>("Acceleration/Accelerate");
        Brake = GetNode<TouchScreenButton>("Acceleration/Brake");
        Left = GetNode<TouchScreenButton>("Steering/Left");
        Right = GetNode<TouchScreenButton>("Steering/Right");
        Esc = GetNode<TextureButton>("Esc");
        Res = GD.Load<resolution>("res://Interface/Res.tres");

        // Setting shadows
        SetShadow(Res.shadows , Res.ShadowQuality);
        //Setting glow
        SetGlow(Res.Glow);
        
        RepositionAndResize(Res.res);
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
        Left.Position = new Vector2(res.x/1920*316 , res.y/1080*10);
        // Left.Scale = new Vector2(0.668f , 0.677f);
        Left.Scale = new Vector2(res.x/1920*1.198f , res.y/1080*1.204f);
        // Right.Position = new Vector2(286 , -127);
        Right.Position = new Vector2(res.x/1920*340 , res.y/1080*-198);
        Right.Scale = Left.Scale;
        // Accelerate_button.Position = new Vector2(-171 , -215);
        Accelerate_button.Position = new Vector2(res.x/1920*-271 , res.y/1080*-362);
        // Accelerate_button.Scale = new Vector2(0.735f , 0.698f);
        Accelerate_button.Scale = new Vector2(res.x/1920*1.271f , res.y/1080*1.232f);
        // Brake.Position = new Vector2(-383 , -121);
        Brake.Position = new Vector2(res.x/1920*-573 , res.y/1080*-166);
        Brake.Scale = Accelerate_button.Scale;
        Esc.SetSize(new Vector2(res.x/1920*96 , res.x/1920*96));
        Esc.SetPosition(new Vector2(res.x/1920*1803 , res.y/1080*18));
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
                ProjectSettings.SetSetting("rendering/quality/directional_shadow/size" , 6400);
                ProjectSettings.SetSetting("rendering/quality/directional_shadow/size.mobile" , 6400);
                break;
            case 3:
                ProjectSettings.SetSetting("rendering/quality/directional_shadow/size" , 8192);
                ProjectSettings.SetSetting("rendering/quality/directional_shadow/size.mobile" , 8192);
                break;
        }
    }
}

