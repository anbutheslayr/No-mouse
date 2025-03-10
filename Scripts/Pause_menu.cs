using Godot;
using System;

public partial class Pause_menu : Control
{
    public VBoxContainer vb1;
    public VBoxContainer vb2;
    public resolution Res;
    public Control inter_face;
    public RigidBody3D plane;
    public bool paused;

    public override void _Ready()
    {
        Res = GD.Load<resolution>("user://Int/Res.tres");

        vb1 = GetNode<VBoxContainer>("MarginContainer/VBoxContainer");
        vb1.AddThemeConstantOverride("separation", (int)(Res.res.y/1080*70));
        vb2 = GetNode<VBoxContainer>("MarginContainer/HBoxContainer/VBoxContainer");
        vb2.AddThemeConstantOverride("separation", (int)(Res.res.y/1080*30));
        inter_face = GetParent().GetNode<Control>("taxi/Interface");
        plane = GetTree().GetNodesInGroup("Plane")[0] as RigidBody3D;
        paused = false;
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
        paused = false;
    }

    public void OnQuitToMMPressed()
    {
        Engine.TimeScale = 1;
        var lod = GetTree().Root.GetNode("LoadingScreen");
        lod.Call("load_scene",GetTree().Root.GetNode("World"),"res://Scenes/Main_menu.tscn");
        // GetTree().ChangeScene("res://Scenes/Main_menu.tscn");
        if(Res.volume != -15)
        {
            var audio_bus = AudioServer.GetBusIndex("Master");
            AudioServer.SetBusMute(audio_bus, false);
        }
        Res.cur_world = 1;
        ResourceSaver.Save("user://Int/Res.tres", Res);
    }
}
