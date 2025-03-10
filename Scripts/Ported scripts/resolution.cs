using Godot;
using System;

public partial class resolution : Resource
{
    [Export] public Vector2 res = new Vector2(1920, 1080);
    [Export] public int res_int = 0;
    [Export] public bool shadows = true;
    [Export] public int ShadowQuality = 0;
    [Export] public bool Glow = true;
    [Export] public int NoOfEnemies = 4;
    [Export] public int volume = 0;
    [Export] public float Drift_points = 0;
    [Export] public int max_worlds = 2;
    [Export] public int cur_world = 1;
    [Export] public int difficulty = 0;

}
