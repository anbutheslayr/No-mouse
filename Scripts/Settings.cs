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
    public TextureButton Esc;
    public OptionButton NoOfEnemies;
    public HSlider volume;
    public OptionButton difficultyselector;
    public override void _Ready()
    {
        Esc = GetNode<TextureButton>("TextureButton");
        Verify_res();
        title_font = GD.Load<DynamicFont>("res://Scenes/FONT.tres");
        theme = GD.Load<Theme>("res://Scenes/Theme.tres");
        Resize(OS.GetScreenSize());
        Reposition(OS.GetScreenSize());

        volume = GetNode<HSlider>("MarginContainer/HBoxContainer2/VBoxContainer/Hslider");
        OptionButton = GetNode<OptionButton>("MarginContainer/HBoxContainer2/VBoxContainer/OptionButton");
        NoOfEnemies = GetNode<OptionButton>("MarginContainer/HBoxContainer2/VBoxContainer/NoOfEnemies");
        shadows = GetNode<OptionButton>("MarginContainer/HBoxContainer2/VBoxContainer/Shadow");
        glow = GetNode<CheckBox>("MarginContainer/HBoxContainer2/VBoxContainer/Glow");
        difficultyselector = GetNode<OptionButton>("MarginContainer/HBoxContainer2/VBoxContainer/Difficulty");
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


        NoOfEnemies.AddItem("1");
        NoOfEnemies.AddItem("2");
        NoOfEnemies.AddItem("3");
        NoOfEnemies.AddItem("4");
        NoOfEnemies.AddItem("5");
        NoOfEnemies.AddItem("6");
        NoOfEnemies.AddItem("7");
        NoOfEnemies.AddItem("8");
        NoOfEnemies.AddItem("9");
        NoOfEnemies.AddItem("10");
        NoOfEnemies.Selected = Res.NoOfEnemies-1;

        difficultyselector.AddItem("Easy", 0);
        difficultyselector.AddItem("Normal",1);
        difficultyselector.AddItem("Hard",2);
        difficultyselector.AddItem("Insane",3);
        difficultyselector.Selected = Res.difficulty;
        volume.Value = Res.volume;
        GD.Print("Volume : " +Res.volume);

    }
    public void OnDifficultySelect(int index)
    {
        switch (index)
        {
            case 0:
                Res.difficulty = 0;
                break;
            case 1: 
                Res.difficulty = 1;
                break;
            case 2:
                Res.difficulty = 2;
                break;
            case 3:
                Res.difficulty = 3;
                break;
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
    public void OnOptionSelect(int index)
    {
        GetParent().Call("Click");
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
        ResourceSaver.Save("user://Int/Res.tres", Res);

        GetParent().Call("Back");
        Hide();
        GetParent().GetNode<Control>("Menu").Show();
        GetParent().Call("Resize");
    }
    public void OnShadowSelect(int index)
    {
        GetParent().Call("Click");
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
        GetParent().Call("Click");
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
                ProjectSettings.SetSetting("rendering/quality/directional_shadow/size" , 4180);
                ProjectSettings.SetSetting("rendering/quality/directional_shadow/size.mobile" , 4180);
                break;
            case 3:
                ProjectSettings.SetSetting("rendering/quality/directional_shadow/size" , 4864);
                ProjectSettings.SetSetting("rendering/quality/directional_shadow/size.mobile" , 4864);
                break;
        }
    }
    public void OnVolumeChange(float value)
    {
        var audio_bus = AudioServer.GetBusIndex("Master");
        AudioServer.SetBusVolumeDb(audio_bus, value);

        if(value == -15)
        {
            AudioServer.SetBusMute(audio_bus, true);
        }
        else
        {
            AudioServer.SetBusMute(audio_bus, false);
        }
        GD.Print("Volume changed to : " +value);
        Res.volume = (int)value;

    }
    public void Reposition(Vector2 resolution)
    {
        vb1 = GetNode<VBoxContainer>("MarginContainer/VBoxContainer");
        vb1.AddConstantOverride("separation", (int)(resolution.y/1080*30));
        hb1 = GetNode<HBoxContainer>("MarginContainer/HBoxContainer");
        hb1.AddConstantOverride("separation", (int)(resolution.x/1920*150));
        vb2 = GetNode<VBoxContainer>("MarginContainer/HBoxContainer/VBoxContainer");
        vb2.AddConstantOverride("separation", (int)(resolution.y/1080*20));
        hb2 = GetNode<HBoxContainer>("MarginContainer/HBoxContainer2");
        hb2.AddConstantOverride("separation", (int)(resolution.x/1920*300));
        vb3 = GetNode<VBoxContainer>("MarginContainer/HBoxContainer2/VBoxContainer");
        vb3.AddConstantOverride("separation", (int)(resolution.y/1080*20));
        Esc.SetPosition(new Vector2(resolution.x/1920*18 , resolution.y/1080*26));
    }
    public void Resize(Vector2 resolution)
    {
        theme.DefaultFont.Set("size", (resolution.x/1920)*50);
        theme.DefaultFont.Set("outline_size", (resolution.x/1920)*3);
        title_font.Size = (int)(resolution.x/1920*100);
        Esc.SetSize(new Vector2(resolution.x/1920*96 , resolution.y/1080*96));
        
    }
    public void OnNumberSelect(int index)
    {
        GetParent().Call("Click");
        Res.NoOfEnemies = index+1;

    }

}
