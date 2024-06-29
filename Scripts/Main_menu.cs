using Godot;
using System;

public class Main_menu : Spatial
{
    public Control settings;
    public Control Menu;
    public resolution Res;
    public override void _Ready()
    {
        Resize();
    }
    public void Resize()
    {
        Res = GD.Load<resolution>("res://Interface/Res.tres");
        Menu = GetNode<Control>("Menu");
        Menu.Call("Resize" , Res.res);

        settings = GetNode<Control>("Settings");
        settings.Call("Resize" , Res.res);
        settings.Call("Reposition" , Res.res);

    }
    // public override void _Process(float delta)
    // {
    //    if(Input.IsActionJustPressed("Save"))
	// 	{
	// 		var image = GetViewport().GetTexture().GetData();
	// 		image.FlipY();
	// 		image.SavePng("D:/Godot export/SS/MainMenu.png");
	// 	}
    // }



}
