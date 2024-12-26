using Godot;
using System;

public class Menu : Control
{
    public VBoxContainer vb1;
    public VBoxContainer vb2;
    public resolution Res;
    public DynamicFont title_font;
    public Theme theme;
    public Control level_select;
    public override void _Ready()
    {
        title_font = GD.Load<DynamicFont>("res://Scenes/FONT.tres");
        theme = GD.Load<Theme>("res://Scenes/Theme.tres");
        level_select = GetParent().GetNode<Control>("Select level");
        Verify_res();
        if (Res.res != Vector2.Zero)
        {
            GetTree().SetScreenStretch(SceneTree.StretchMode.Viewport, SceneTree.StretchAspect.Expand, Res.res);
            GetParent().Call("Resize");
            Resize(Res.res);
            OS.WindowSize = Res.res;
        }
        else
        {
            Res.res = OS.GetScreenSize();
            Res.res_int = 0;
            GetParent().Call("Resize");
            Resize(Res.res);
        }
        if(Res.ShadowQuality != 4)
        {
            Res.shadows = true;
            GetParent().GetNode<DirectionalLight>("DirectionalLight").ShadowEnabled = true;
            SetShadowQuality(Res.ShadowQuality);
        }
        else
        {
            Res.shadows = false;
            GetParent().GetNode<DirectionalLight>("DirectionalLight").ShadowEnabled = false;
        }
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
    
    public void OnPlayPressed()
    {
        GetParent().Call("Click");
        ResourceSaver.Save("res://Interface/Res.tres", Res);
        Hide();
        level_select.Show();
    }
    public void OnSettingsPressed()
    {
        Hide();
        GetParent().GetNode<Control>("Settings").Show();
        GetParent().Call("Click");
        
    }

    public void OnQuitPressed()
    {
        GetParent().Call("Back");
        ResourceSaver.Save("res://Interface/Res.tres", Res);
        GetTree().Quit();
    }
    public void Resize(Vector2 resolution)
    {
        theme.DefaultFont.Set("size", (resolution.x/1920)*30);
        theme.DefaultFont.Set("outline_size", (resolution.x/1920)*4);
        title_font.Size = (int)(resolution.x/1920*64);

        vb1 = GetNode<VBoxContainer>("MarginContainer/VBoxContainer");
        vb1.AddConstantOverride("separation", (int)(resolution.y/1080*70));
        vb2 = GetNode<VBoxContainer>("MarginContainer/HBoxContainer/VBoxContainer");
        vb2.AddConstantOverride("separation", (int)(resolution.y/1080*30));
    }
    public void SetShadowQuality(int index)
    {
        switch(index)
        {
            case 0:
                ProjectSettings.SetSetting("rendering/quality/directional_shadow/size" , 2048);
                ProjectSettings.SetSetting("rendering/quality/directional_shadow/size.mobile" , 2048);
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
    public void On_insta_pressed()
    {
        OS.ShellOpen("https://www.instagram.com/anbu_the_coder/profilecard/?igsh=MTBmbm83Z3hwcjI3bw%3D%3D");
    }
}
