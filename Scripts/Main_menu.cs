using Godot;
using System;

public class Main_menu : Spatial
{
    public Control Menu;
    public resolution Res;
    public override void _Ready()
    {
        Res = GD.Load<resolution>("res://Interface/Res.tres");
        Menu = GetNode<Control>("Menu");
        Menu.Call("Resize" , Res.res);
    }
    


}
