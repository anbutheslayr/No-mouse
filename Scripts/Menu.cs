using Godot;
using System;

public class Menu : Control
{
    public void OnPlayPressed()
    {
        GetTree().ChangeScene("res://Scenes/World.tscn");
    }
    public void OnSettingsPressed()
    {

    }

    public void OnQuitPressed()
    {
        GetTree().Quit();
    }
}
