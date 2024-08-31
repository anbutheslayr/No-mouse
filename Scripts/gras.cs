using Godot;
using System;

public class gras : MultiMeshInstance
{
    [Export] public NodePath characterPath;
    private Spatial character;

    public override void _Ready()
    {
        character = GetNode<Spatial>(characterPath); 
    }
 // Called every frame. 'delta' is the elapsed time since the previous frame.
 public override void _Process(float delta)
 {
    MaterialOverride.Set("character_position", character.GlobalTransform.origin);
    MaterialOverride._Set("character_position", character.GlobalTransform.origin);
    GD.Print(character.GlobalTransform.origin);
 }
}
