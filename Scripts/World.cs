using Godot;
using System;

public class World : Spatial
{
    public PackedScene car_scene;
    [Export] public string car_scene_path;
    public override void _Ready()
    {
        GD.Print("sTART");
        car_scene = (PackedScene)ResourceLoader.Load(car_scene_path);
        AddChild(car_scene.Instance());
    }
}
