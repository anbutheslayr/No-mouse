using Godot;
using System;

public partial class Follow : Camera3D
{
    [Export] public float lerp_speed = 3;
    [Export] public string target_PATH;
    [Export] public Vector3 offset;
    public MeshInstance3D target;
    public Vector3 target_pos;
    public float trauma = 0;
    [Export] public float trauma_red_rate = 1;
    [Export] public FastNoiseLite noise;
    public float time = 0;
    [Export]public int noise_speed = 50;
    [Export] public float max_x = 10;
    [Export] public float max_y = 10;
    [Export] public float max_z = 5;
    public RayCast3D raycast;
    [Export] public int col_lerp_speed = 10;
    public Node3D rayp;
    public Gun gun;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        target = GetParent().GetNode<MeshInstance3D>(target_PATH);
        raycast = new RayCast3D();
        AddChild(raycast);
        raycast.Enabled = true;
        raycast.AddException(this);
        raycast.AddException(target);
        raycast.CollideWithAreas = false;
        raycast.CollideWithBodies = true;
        rayp = target.GetNode<Node3D>("Raycol");
        gun = target.GetNode<Gun>("body/MachineGun");
    }
    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _PhysicsProcess(float delta)
    {

        // Transform target_pos = target.GlobalTransform.Translated(offset);
        // GlobalTransform = GlobalTransform.InterpolateWith(target_pos, lerp_speed * delta);
        var target_pos = target.GlobalTransform.Translated(offset).origin;
        target_pos.y = Mathf.Max(target_pos.y, target.GlobalTransform.origin.y + offset.y);

        raycast.GlobalTranslation = rayp.GlobalTranslation;
        var dir = GlobalTransform.origin - raycast.GlobalTransform.origin;
        raycast.TargetPosition = dir;
        raycast.ForceRaycastUpdate();
        // if (raycast.IsColliding() && raycast.GetCollider() is StaticBody)
        if(gun.closest_enemy != null || raycast.GetCollider() is StaticBody3D)
        {
            GlobalTranslation = GlobalTranslation.Lerp( new Vector3(target_pos.x, 25, target_pos.z),col_lerp_speed*delta);
        }
        else
        {
            GlobalTranslation = GlobalTranslation.Lerp(target_pos, lerp_speed * delta);
        }
        LookAt(target.GlobalTransform.origin , Vector3.Up);
        
        trauma = Mathf.Max(trauma - trauma_red_rate * delta , 0);
        time += delta;
        RotationDegrees = new Vector3(RotationDegrees.x + GetNoiseFromSeed(0)*max_x*GetShakeIntensity(),
        RotationDegrees.y+ GetNoiseFromSeed(1)*max_y*GetShakeIntensity(),
        RotationDegrees.z + GetNoiseFromSeed(2)*max_z*GetShakeIntensity());
    }
    public void Add_trauma(float amount)
    {
        trauma = Mathf.Clamp(trauma + amount ,0, 1);
    }
    public float GetNoiseFromSeed(int seed)
    {
        noise.Seed = seed;
        return noise.GetNoise1d(time * noise_speed);
    }
    public float GetShakeIntensity()
    {
        return trauma*trauma;
    }
}
