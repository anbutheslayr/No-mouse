using Godot;
using System;

public class Select_level : Control
{
    public resolution Res;
    public AudioStreamPlayer play;
    public Global_vars gl;
    public override void _Ready()
    {
        Res = ResourceLoader.Load<resolution>("user://Int/Res.tres");
        gl = GetNode<Global_vars>("/root/GlobalVars");
    }
    public void On_World1()
    {
        Res.cur_world = 1;
        ResourceSaver.Save("user://Int/Res.tres", Res);
        // gl.SceneToLoad = "res://Scenes/Worlds/World.tscn";
        // gl.currentscene = GetTree().Root.GetNode<Spatial>("Main_menu");
        var lod = GetTree().Root.GetNode("LoadingScreen");
        lod.Call("load_scene",GetTree().Root.GetNode("Main_menu"),"res://Scenes/Worlds/World.tscn");
        // GetTree().Root.AddChild(ResourceLoader.Load<PackedScene>("res://Interface/Loading screen.tscn").Instance());
        // QueueFree();
        // GetTree().ChangeScene("res://Scenes/Worlds/World.tscn");
    }
    public void On_World2()
    {
        Res.cur_world = 2;
        ResourceSaver.Save("user://Int/Res.tres", Res);
        // gl.SceneToLoad = "res://Scenes/Worlds/World2.tscn";
        var lod = GetTree().Root.GetNode("LoadingScreen");
        lod.Call("load_scene",GetTree().Root.GetNode("Main_menu"),"res://Scenes/Worlds/World2.tscn");
        // GetTree().Root.AddChild(ResourceLoader.Load<PackedScene>("res://Interface/Loading screen.tscn").Instance());
        // GetTree().ChangeScene("res://Scenes/Worlds/World2.tscn");
    }
    public void On_Cutscene()
    {
        var lod = GetTree().Root.GetNode("LoadingScreen");
        lod.Call("load_scene",GetTree().Root.GetNode("Main_menu"),"res://Scenes/Worlds/Cutscene.tscn");
    }
    
}
