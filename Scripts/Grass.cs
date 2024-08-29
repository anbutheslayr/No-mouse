using Godot;
using System;

public class Grass : MultiMeshInstance
{
    [Export] public Vector2 extends;
    [Export] public bool spawnOutsideCircle = false;
    [Export] public float radius = 12.0f;
    [Export] public NodePath characterPath;
    private Spatial character;

    public override void _Ready()
    {
        character = GetNode<Spatial>(characterPath); 
    }

    public override void _EnterTree()
    {
        Connect("visibility_changed", this, nameof(_on_visibility_changed));
        var rng = new RandomNumberGenerator();
        rng.Randomize();
        var center = GlobalTransform.origin;

        int instanceIndex = 0;
        int spawnCount = Multimesh.InstanceCount;

        while (instanceIndex < spawnCount)
        {
            float x, z;
            if (spawnOutsideCircle)
            {
                x = center.x + (radius + rng.RandfRange(0, extends.x));
                z = center.z + (radius + rng.RandfRange(0, extends.y));
            }
            else
            {
                var theta = rng.RandfRange(0, 2 * Mathf.Pi);
                x = center.x + (radius + rng.RandfRange(0, extends.x)) * Mathf.Cos(theta);
                z = center.z + (radius + rng.RandfRange(0, extends.y)) * Mathf.Sin(theta);
            }

            Vector3 position = new Vector3(x, 0, z);

            if (!IsCollidingWithStaticBody(position))
            {
                var transform = new Transform();
                transform.origin = position;
                Multimesh.SetInstanceTransform(instanceIndex, transform);
                instanceIndex++;
            }
        }
    }

    public override void _Process(float delta)
    {
        MaterialOverride.Set("character_position", character.GlobalTransform.origin);
    }

    private void _on_visibility_changed()
    {
        if (Visible)
        {
            _Ready();
        }
    }

private bool IsCollidingWithStaticBody(Vector3 position)
{
    PhysicsDirectSpaceState spaceState = GetWorld().DirectSpaceState;

    // Create a small sphere shape for collision detection
    SphereShape testShape = new SphereShape
    {
        Radius = 0.1f  // Adjust based on the size of your grass strands
    };

    // Setup the query parameters
    PhysicsShapeQueryParameters queryParams = new PhysicsShapeQueryParameters();
    queryParams.Transform = new Transform(Basis.Identity, position);
    queryParams.SetShape(testShape);
    queryParams.CollisionMask = 1;  // Adjust to match the collision layer/mask of your StaticBody

    // Perform the collision check
    Godot.Collections.Array result = spaceState.IntersectShape(queryParams);

    // Check if there was a collision with a StaticBody
    foreach (Godot.Collections.Dictionary collision in result)
    {
        if ((collision["collider"] as Node).IsInGroup("Ground"))
        {
            GD.Print("Collided with StaticBody");
            return true;
        }
    }

    return false;
}

}
