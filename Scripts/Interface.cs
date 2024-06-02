using Godot;
using System;

public class Interface : Control
{
    public void OnEscPressed()
    {
        GetTree().ChangeScene("res://Scenes/Main_menu.tscn");
    }
}
