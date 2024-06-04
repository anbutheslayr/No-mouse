using Godot;
using System;

public class Settings : Control
{
    public OptionButton OptionButton;
    public OptionButton shadows;
    public CheckBox glow;
    public resolution Res;
    public Theme theme;
    // multiple res support
    public VBoxContainer vb1;
    public HBoxContainer hb1;
    public VBoxContainer vb2;
    public HBoxContainer hb2;
    public VBoxContainer vb3;
    public Label title;
    public DynamicFont title_font;
    public override void _Ready()
    {
        title_font = GD.Load<DynamicFont>("res://Scenes/FONT.tres");
        theme = GD.Load<Theme>("res://Scenes/Theme.tres");
        Resize(OS.GetScreenSize());
        Reposition(OS.GetScreenSize());

        Res = GD.Load<resolution>("res://Interface/Res.tres");
        OptionButton = GetNode<OptionButton>("MarginContainer/HBoxContainer2/VBoxContainer/OptionButton");
        shadows = GetNode<OptionButton>("MarginContainer/HBoxContainer2/VBoxContainer/Shadow");
        glow = GetNode<CheckBox>("MarginContainer/HBoxContainer2/VBoxContainer/Glow");
        if(Res.Glow)
        {
            glow.SetPressedNoSignal(true);
        }

        OptionButton.AddItem("Default : " + OS.GetScreenSize().ToString(), 0);
        OptionButton.AddItem("1920x1080", 1);
        OptionButton.AddItem("1280x720", 2);
        OptionButton.AddItem("1024x576",3);
        OptionButton.AddItem("960x540", 4);
        // OptionButton.AddItem("640x360", 5);
        OptionButton.Selected = Res.res_int;


        shadows.AddItem("Low Quality", 0);
        shadows.AddItem("Medium Quality", 1);
        shadows.AddItem("High Quality", 2);
        shadows.AddItem("Ultra Quality", 3);
        shadows.AddItem("Disabled", 4);
        shadows.Selected = Res.ShadowQuality;

    }
    public void OnOptionSelect(int index)
    {
        Res.res_int = index;
        if ( index == 0)
        {
            GetTree().SetScreenStretch(SceneTree.StretchMode.Disabled, SceneTree.StretchAspect.Expand, OS.WindowSize);
            Res.res = OS.GetScreenSize();
            Resize(OS.GetScreenSize());
            Reposition(OS.GetScreenSize());
        }
        else
        {
            var size = new Vector2(OptionButton.GetItemText(OptionButton.Selected).ToString().Split('x')[0].ToFloat(), OptionButton.GetItemText(OptionButton.Selected).ToString().Split('x')[1].ToFloat());
            GetTree().SetScreenStretch(SceneTree.StretchMode.Viewport, SceneTree.StretchAspect.Expand, size);
            OS.WindowSize = size;
            Res.res = size;
            Resize(size);
            Reposition(size);   
        }
        
    }
    public void OnEsc()
    {
        Hide();
        GetParent().GetNode<Control>("Menu").Show();
    }
    public void OnShadowSelect(int index)
    {
        Res.ShadowQuality = index;
        if(index != 4)
        {
            Res.shadows = true;
            GetParent().GetNode<DirectionalLight>("DirectionalLight").ShadowEnabled = true;
            SetShadowQuality(index);
        }
        else
        {
            Res.shadows = false;
            GetParent().GetNode<DirectionalLight>("DirectionalLight").ShadowEnabled = false;
        }
    }
    public void OnGlowToggled(bool enabled)
    {
        if(enabled)
        {
            Res.Glow = true;
            GetTree().Root.GetNode<WorldEnvironment>("Main_menu/WorldEnvironment").Environment.GlowEnabled = true;
        }
        else
        {
            Res.Glow = false;
            GetTree().Root.GetNode<WorldEnvironment>("Main_menu/WorldEnvironment").Environment.GlowEnabled = false;
        }
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
    public void Reposition(Vector2 resolution)
    {
        vb1 = GetNode<VBoxContainer>("MarginContainer/VBoxContainer");
        vb1.AddConstantOverride("separation", (int)(resolution.y/1080*30));
        hb1 = GetNode<HBoxContainer>("MarginContainer/HBoxContainer");
        hb1.AddConstantOverride("separation", (int)(resolution.x/1920*50));
        vb2 = GetNode<VBoxContainer>("MarginContainer/HBoxContainer/VBoxContainer");
        vb2.AddConstantOverride("separation", (int)(resolution.y/1080*20));
        hb2 = GetNode<HBoxContainer>("MarginContainer/HBoxContainer2");
        hb2.AddConstantOverride("separation", (int)(resolution.x/1920*300));
        vb3 = GetNode<VBoxContainer>("MarginContainer/HBoxContainer2/VBoxContainer");
        vb3.AddConstantOverride("separation", (int)(resolution.y/1080*20));
    }
    public void Resize(Vector2 resolution)
    {
        theme.DefaultFont.Set("size", (resolution.x/1920)*30);
        theme.DefaultFont.Set("outline_size", (resolution.x/1920)*4);

        title_font.Size = (int)(resolution.x/1920*64);
        
    }

}
