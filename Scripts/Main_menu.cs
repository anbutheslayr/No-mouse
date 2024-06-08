using Godot;
using System;
using System.Drawing.Imaging;

public class Main_menu : Spatial
{
    public Control settings;
    public Control Menu;
    public resolution Res;
    public Control Res_select;
    public Control play;
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
    public void SettingsFocus()
    {
        Res_select = GetNode<Control>("Settings/MarginContainer/HBoxContainer2/VBoxContainer/OptionButton");
        Res_select.GrabFocus();
    }
    public void MenuFocus()
    {
        play = GetNode<Control>("Menu/MarginContainer/HBoxContainer/VBoxContainer/Play");
        play.GrabFocus();
    }
    


}
