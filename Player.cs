using Godot;
using System;

public class Player : KinematicBody
{
	
	[Export] public float brake;
	[Export] public float Speed;
	[Export] Camera camera;
	public float steering = 0;
	public override void _Ready()
	{
		GD.Print("sTART");
	}
	public override void _UnhandledInput(InputEvent @event)
{
    if (@event is InputEventScreenTouch touch)
        if (touch.IsPressed())
		{
			Viewport View = camera.GetViewport();
		}
}
 // Called every frame. 'delta' is the elapsed time since the previous frame.
 	public override void _Process(float delta)
 	{
		 	
	}
}
