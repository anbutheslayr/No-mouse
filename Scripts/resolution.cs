using Godot;
using System;

public class resolution : Resource
{
    [Export] public Vector2 res = Vector2.Zero;
    [Export] public int res_int = 0;
    [Export] public bool shadows = true;
    [Export] public int ShadowQuality = 1;
    [Export] public bool Glow = true;

}
