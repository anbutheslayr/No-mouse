using Godot;
using System;

public class Menu : Control
{
    public VBoxContainer vb1;
    public VBoxContainer vb2;
    public resolution Res;
    public DynamicFont title_font;
    public Theme theme;
    public int selected = 0;
    public Button play;
    public Button settings;
    public Button quit;
    public override void _Ready()
    {
        play = GetNode<Button>("MarginContainer/HBoxContainer/VBoxContainer/Play");
        play.GrabFocus();
        settings = GetNode<Button>("MarginContainer/HBoxContainer/VBoxContainer/Settings");
        quit = GetNode<Button>("MarginContainer/HBoxContainer/VBoxContainer/Quit");
        title_font = GD.Load<DynamicFont>("res://Scenes/FONT.tres");
        theme = GD.Load<Theme>("res://Scenes/Theme.tres");
        Res = GD.Load<resolution>("res://Interface/Res.tres");
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
    
    public void OnPlayPressed()
    {
        ResourceSaver.Save("res://Interface/Res.tres", Res);
        GetTree().ChangeScene("res://Scenes/World.tscn");
    }
    public void OnSettingsPressed()
    {
        Hide();
        GetParent().GetNode<Control>("Settings").Show();
        GetParent().Call("SettingsFocus");
    }

    public void OnQuitPressed()
    {
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
    public override void _Input(InputEvent @event)
    {
        if(@event is InputEventKey && @event.IsActionPressed("ui_cancel") && !GetParent().GetNode<Control>("Settings").IsVisibleInTree())
        {
            OnQuitPressed();
        }
    }


}
