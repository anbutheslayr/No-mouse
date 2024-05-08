using Godot;
using System;

public class Follow : Camera
{
    [Export] public float lerp_speed = 3;
    [Export] public string target_PATH;
    [Export] public Vector3 offset;
    public MeshInstance target;
    public Label fpsLabel = new Label();

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        target = GetParent().GetNode<MeshInstance>(target_PATH);
        // Create a new label node
        AddChild(fpsLabel);

        // Set the label's position to the top left corner of the screen
        fpsLabel.RectMinSize = new Vector2(100, 20);
        fpsLabel.RectPosition = new Vector2(10, 10);
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _PhysicsProcess(float delta)
    {
       var target_pos = target.GlobalTransform.Translated(offset);
       GlobalTransform = GlobalTransform.InterpolateWith(target_pos , lerp_speed * delta);
       LookAt(target.GlobalTransform.origin , Vector3.Up);
    }

    public override void _Process(float delta)
    {
        // Update the label's text with the current FPS value
        fpsLabel.Text = "Fps : " + Engine.GetFramesPerSecond().ToString();
    }
}
