using Godot;
using System;

public class Healthbar3d : Spatial
{
	[Export] public string texture_progress_path;
    [Export] public string tween_path;
    public Tween tween;
	public TextureProgress texture_progress;
	public override void _Ready()
	{
		texture_progress = GetNode<TextureProgress>(texture_progress_path);
		texture_progress.Value = 100;
        tween = GetNode<Tween>(tween_path);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public void Change_Health(int health)
	{
		// texture_progress.Value = health;
        tween.InterpolateProperty(texture_progress , "value" , texture_progress.Value , health, 0.3f, Tween.TransitionType.Back, Tween.EaseType.Out);
        tween.Start();
	}
}
