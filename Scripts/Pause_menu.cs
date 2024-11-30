using Godot;
using System;

public class Pause_menu : Control
{
    public VBoxContainer vb1;
    public VBoxContainer vb2;
    public resolution Res;
    public Control inter_face;
    public RigidBody plane;

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

    public void OnResumePressed()
    {
        inter_face.Show();
        Engine.TimeScale = 1;
        Hide();
        plane.Call("Resume");
        if(Res.volume != -15)
        {
            var audio_bus = AudioServer.GetBusIndex("Master");
            AudioServer.SetBusMute(audio_bus, false);
        }
    }

    public void OnQuitToMMPressed()
    {
        Engine.TimeScale = 1;
        GetTree().ChangeSceneTo(null);
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
