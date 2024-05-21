using Godot;
using System.Collections.Generic;

public class Machine_gun : Spatial
{
    [Export] public string Det_area_path;
    public Area Det_area;
    public List<Spatial> enemies = new List<Spatial>();

    public override void _Ready()
    {
        Det_area = GetNode<Area>(Det_area_path);
    }

    public void OnDetection(Node body)
    {
        if(body.IsInGroup("Enemy"))
        {
            enemies.Add(body as Spatial);
        }
    }
    public void OnExit(Node body)
    {
        if(body.IsInGroup("Enemy"))
        {
            enemies.Remove(body as Spatial);
        }
    }
    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        if(enemies.Count > 0)
        {
            Spatial closest_enemy = GetClosestEnemy();
            if(closest_enemy != null)
            {
                AimAt(closest_enemy.GlobalTransform.origin, Vector3.Up);
            }
        }
    }
    public Spatial GetClosestEnemy()
    {
        Spatial closest_enemy = null;
        float closest_distance = 0;
        foreach(Spatial enemy in enemies)
        {
            float distance = GlobalTransform.origin.DistanceTo(enemy.GlobalTransform.origin);
            if(closest_enemy == null || distance < closest_distance)
            {
                closest_enemy = enemy;
                closest_distance = distance;
            }
        }
        return closest_enemy;
    }
    private void AimAt(Vector3 target, Vector3 up)
    {
        Vector3 direction = target - GlobalTransform.origin;
        LookAt(GlobalTransform.origin + direction, up);
    }
}
