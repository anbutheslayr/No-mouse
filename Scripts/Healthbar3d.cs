using Godot;
using System;

public class Healthbar3d : Spatial
{
	[Export] public string texture_progress_path;
    [Export] public string tween_path;
    [Export] public string texture_progress_under_path;
    public Tween tween;
	public TextureProgress texture_progress;
    public TextureProgress texture_progress_under;
	public override void _Ready()
	{
		texture_progress = GetNode<TextureProgress>(texture_progress_path);
        texture_progress_under = GetNode<TextureProgress>(texture_progress_under_path);
		texture_progress.Value = 100;
        tween = GetNode<Tween>(tween_path);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public void Change_Health(int health)
	{
		texture_progress.Value = health;
        tween.InterpolateProperty(texture_progress , "value" , texture_progress.Value , health, 0.15f, Tween.TransitionType.Elastic, Tween.EaseType.In);
        tween.InterpolateProperty(texture_progress_under , "value" , texture_progress_under.Value , health, 0.7f, Tween.TransitionType.Cubic, Tween.EaseType.In);
        tween.Start();
	}
}
