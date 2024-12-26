using Godot;
using System;

public class Death_screen : Control
{   
    public VBoxContainer vb1;
    public VBoxContainer vb2;
    public resolution Res;
    public Control inter_face;
    public RigidBody plane;
    public bool Started = false;


    public override void _Ready()
    {
        Res = GD.Load<resolution>("user://Int/Res.tres");

        vb1 = GetNode<VBoxContainer>("MarginContainer/VBoxContainer");
        vb1.AddConstantOverride("separation", (int)(Res.res.y/1080*70));
        vb2 = GetNode<VBoxContainer>("MarginContainer/HBoxContainer/VBoxContainer");
        vb2.AddConstantOverride("separation", (int)(Res.res.y/1080*30));
        inter_face = GetParent().GetNode<Control>("taxi/Interface");
        plane = GetTree().GetNodesInGroup("Plane")[0] as RigidBody;

    }
    public override void _Process(float delta)
    {
        if(Started)
        {
            Engine.TimeScale = Mathf.Lerp(Engine.TimeScale, 0, 0.03f);
        }
        if(Engine.TimeScale < 0.1)
        {
            if(Res.volume != -15)
            {
                var audio_bus = AudioServer.GetBusIndex("Master");
                AudioServer.SetBusMute(audio_bus, true);
            }
        }
    }
    public void Start()
    {
        Started = true;
    }

    public void OnRestartPressed()
    {
        Res = GD.Load<resolution>("user://Int/Res.tres");
        Engine.TimeScale = 1;
        if(Res.volume != -15)
        {
            var audio_bus = AudioServer.GetBusIndex("Master");
            AudioServer.SetBusMute(audio_bus, false);
        }
        if(Res.cur_world == 1)
            GetTree().ChangeScene("res://Scenes/Worlds/World.tscn");
        else
            GetTree().ChangeScene("res://Scenes/Worlds/World"+Res.cur_world+".tscn");
    }

    public void OnQuitToMMPressed()
    {
        Engine.TimeScale = 1;
        GetTree().ChangeScene("res://Scenes/Main_menu.tscn");
        if(Res.volume != -15)
        {
            var audio_bus = AudioServer.GetBusIndex("Master");
            AudioServer.SetBusMute(audio_bus, false);
        }
        Res.cur_world = 1;
        ResourceSaver.Save("user://Int/Res.tres", Res);
    }
}