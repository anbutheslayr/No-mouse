using Godot;
using System;

public class Machine_gun : Spatial
{
    [Export] public string Det_area_path;
    public Area Det_area;
    public override void _Ready()
    {
        Det_area = GetNode<Area>(Det_area_path);
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        
    }
}
