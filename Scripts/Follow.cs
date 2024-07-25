using Godot;
using System;

public class Follow : Camera
{
    [Export] public float lerp_speed = 3;
    [Export] public string target_PATH;
    [Export] public Vector3 offset;
    public Vector3 cur_offset;
    public MeshInstance target;
    public Spatial marker;
    public RayCast raycast;
    public Vector3 target_pos;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        target = GetParent().GetNode<MeshInstance>(target_PATH);
        raycast = GetNode<RayCast>("RayCast");
        marker = GetParent().GetNode<Spatial>("taxi/Marker");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _PhysicsProcess(float delta)
    {
        foreach (Node obj in GetTree().GetNodesInGroup("Leaves"))
        {
            if (obj is MeshInstance mesh)
            {
                ShaderMaterial mat = (ShaderMaterial)mesh.MaterialOverride;

                if (mat != null)
                {
                    mat.SetShaderParam("player_position", target.GlobalTransform.origin);
                    mat.SetShaderParam("camera_position", GlobalTransform.origin);
                    GD.Print("Set");
                }
            }
        }
       cur_offset = offset; 
       var target_pos = target.GlobalTransform.Translated(cur_offset);
       GlobalTransform = GlobalTransform.InterpolateWith(target_pos , lerp_speed * delta);
       LookAt(target.GlobalTransform.origin , Vector3.Up);
    }
}
