using Godot;
using System;

public class Menu : Control
{
    public VBoxContainer vb1;
    public VBoxContainer vb2;
    public resolution Res;
    public override void _Ready()
    {

        Res = GD.Load<resolution>("res://Interface/Res.tres");
        if (Res.res != Vector2.Zero)
        {
            Resize(Res.res);
        }
        else
        {
            Resize(OS.GetScreenSize());
        }
    }
    
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
    public void Resize(Vector2 resolution)
    {
        vb1 = GetNode<VBoxContainer>("MarginContainer/VBoxContainer");
        vb1.AddConstantOverride("separation", (int)(resolution.y/1080*70));
        vb2 = GetNode<VBoxContainer>("MarginContainer/HBoxContainer/VBoxContainer");
        vb2.AddConstantOverride("separation", (int)(resolution.y/1080*30));
    }
}
