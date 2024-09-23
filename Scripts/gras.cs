using Godot;
using System;

public class gras : MeshInstance
{
	[Export] public NodePath characterPath;
	public MultiMeshInstance mmi;
	public ShaderMaterial mat;
	private Spatial character;

	public override void _Ready()
	{
		character = GetNode<Spatial>(characterPath); 
		mat = GD.Load<ShaderMaterial>("res://Assets/wind_grass.tres");
		mmi = GetParent().GetNode<MultiMeshInstance>("Gardener/Arborist/MMI_container/MultiMeshInstance");
		// for (int i = 0; i < mmi.GetChildCount(); i++)
		// {
		// 	(mmi.GetChild(i) as MultiMeshInstance).MaterialOverride = MaterialOverride;
		// }
	}
 // Called every frame. 'delta' is the elapsed time since the previous frame.
 public override void _Process(float delta)
 {
	mmi.MaterialOverride.Set("character_position", character.GlobalTransform.origin);
	// GD.Print(character.GlobalTransform.origin);
 }
}
