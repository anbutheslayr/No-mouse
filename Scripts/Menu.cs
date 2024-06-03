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
        Hide();
        GetParent().GetNode<Control>("Settings").Show();
    }

    public void OnQuitPressed()
    {
        GetTree().Quit();
    }
}
