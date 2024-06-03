using Godot;
using System;

public class Settings : Control
{
    public OptionButton OptionButton;
    public OptionButton shadows;
    public CheckBox glow;
    public resolution Res;
    public override void _Ready()
    {
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
        OptionButton.AddItem("640x360", 5);
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
        }
        else
        {
            var size = new Vector2(OptionButton.GetItemText(OptionButton.Selected).ToString().Split('x')[0].ToFloat(), OptionButton.GetItemText(OptionButton.Selected).ToString().Split('x')[1].ToFloat());
            GetTree().SetScreenStretch(SceneTree.StretchMode.Viewport, SceneTree.StretchAspect.Expand, size);
            OS.WindowSize = size;
            Res.res = size;   
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
