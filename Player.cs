using Godot;
using System;
using System.Threading;

public class Player : VehicleBody
{
	
	[Export] public float brake;
	[Export] public float Speed = 50f;
	[Export] public string camera_path;
	[Export] public string car_path;
	public VehicleWheel F_left;
	public VehicleWheel B_left;
	public VehicleWheel F_right;
	public VehicleWheel B_right;
	VehicleBody car;
	Camera camera;
	public float steering = 0;
	public override void _Ready()
	{
		GD.Print("sTART");
		camera = GetNode<Camera>(camera_path);
		car = GetNode<VehicleBody>(car_path);
		F_left = GetNode<VehicleWheel>("F_left");
		F_right = GetNode<VehicleWheel>("F_right");
		B_left = GetNode<VehicleWheel>("B_left");
		B_right = GetNode<VehicleWheel>("B_right");
		B_left.UseAsTraction = true;
		B_right.UseAsTraction = true;
	}
	public override void _UnhandledInput(InputEvent @event)
{
	if (@event is InputEventScreenTouch touch)
		if (touch.IsPressed())
		{
			Viewport View = camera.GetViewport();
			var canvas = View.Size;
			if(touch.Position.x > canvas.x/2 )
			{
				steering = 60;
			}
			else
			{
				steering = -60;
			}
			F_left.Steering = steering;
			F_right.Steering = steering;
		}
		else
		{
			steering = 0;
		}
}
 // Called every frame. 'delta' is the elapsed time since the previous frame.
 	public override void _Process(float delta)
 	{
		B_left.EngineForce = Speed;
		B_right.EngineForce = Speed;

	}
}
