using Godot;
using System;

public class Main_menu : Spatial
{
    public Control settings;
    public Control Menu;
    public resolution Res;
    public AudioStreamPlayer click;
    public AudioStreamPlayer back;
    public override void _Ready()
    {
        Resize();
    }
    public void Verify_res()
    {
        var dir = new Directory();
        dir.Open("user://");
        if(!dir.DirExists("user://Int")) 
        {
            dir.MakeDir("user://Int");
            GD.Print("user://Int created");
        }

        if(dir.FileExists("user://Int/Res.tres"))
        {
            Res = ResourceLoader.Load<resolution>("user://Int/Res.tres");
        }
        else
        {
            Res = ResourceLoader.Load<resolution>("res://Interface/Res.tres");
            GD.Print("Res created");
            ResourceSaver.Save("user://Int/Res.tres", Res);
        }
    }
    public void Resize()
    {
        Verify_res();
        Menu = GetNode<Control>("Menu");
        Menu.Call("Resize" , Res.res);

        settings = GetNode<Control>("Settings");
        settings.Call("Resize" , Res.res);
        settings.Call("Reposition" , Res.res);
        click = GetNode<AudioStreamPlayer>("Click");
        back = GetNode<AudioStreamPlayer>("Back");
    }
    public void Click()
    {
        click.Play();
    }
    public void Back()
    {
        back.Play();
    }
    public override void _Process(float delta)
    {
       if(Input.IsActionJustPressed("Save"))
		{
			var image = GetViewport().GetTexture().GetData();
			image.FlipY();
			image.SavePng("D:/Godot export/SS/MainMenu.png");
		}
    }
}
