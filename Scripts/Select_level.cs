using Godot;
using System;

public class Select_level : Control
{
    public resolution Res;
    public override void _Ready()
    {
        Res = ResourceLoader.Load<resolution>("user://Int/Res.tres");
    }
    public void On_World1()
    {
        Res.cur_world = 1;
        ResourceSaver.Save("user://Int/Res.tres", Res);
        GetTree().ChangeScene("res://Scenes/Worlds/World.tscn");
    }
    public void On_World2()
    {
        Res.cur_world = 2;
        ResourceSaver.Save("user://Int/Res.tres", Res);
        GetTree().ChangeScene("res://Scenes/Worlds/World2.tscn");
    }
}
