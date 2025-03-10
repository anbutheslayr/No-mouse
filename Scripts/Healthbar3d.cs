using Godot;
using System;

public partial class Healthbar3d : Node3D
{
	[Export] public string texture_progress_path;
    [Export] public string texture_progress_under_path;
    public Tween tween;
	public TextureProgressBar texture_progress;
    public TextureProgressBar texture_progress_under;
	public override void _Ready()
	{
		texture_progress = GetNode<TextureProgressBar>(texture_progress_path);
        texture_progress_under = GetNode<TextureProgressBar>(texture_progress_under_path);
        tween = GetNode<Tween>("Tween");
	}

	public void Change_Health(int health,bool immediate)
	{
		if(immediate)
		{
			texture_progress.Value = health;
        	tween.InterpolateProperty(texture_progress , "value" , texture_progress.Value , health, 0.5f, Tween.TransitionType.Elastic, Tween.EaseType.In);
			tween.InterpolateProperty(texture_progress_under , "value" , texture_progress_under.Value , health, 0.7f, Tween.TransitionType.Cubic, Tween.EaseType.In);
        	tween.Start();
		}
		else
		{
			tween.InterpolateProperty(texture_progress , "value" , texture_progress.Value , health, 0.20f, Tween.TransitionType.Elastic, Tween.EaseType.In);
			tween.InterpolateProperty(texture_progress_under , "value" , texture_progress_under.Value , health, 0.7f, Tween.TransitionType.Cubic, Tween.EaseType.In);
        	tween.Start();
		}
		if(health <= 0)
		{
			GetParent().GetParent().GetNode<Control>("Interface").Call("Dead");	
		}
	}
}
