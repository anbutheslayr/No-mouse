using Godot;
using System;

public partial class Skeleton3D : Node3D
{
    public Node3D healthbar;
    public Node3D player_mesh;
    public Node3D character;
    public RigidBody3D ball;
    public Vector3 sphere_offset = new Vector3(0, -0.9f, 0);
    public NavigationAgent3D agent;
    [Export]public float speed_input;
    public Vector3 nexpos;
    public Navigation navigation;
    public Timer update_path_timer;
    public float steering_input;
    [Export] public float steering = 60;
    [Export] public int acceleration = 25;
    [Export] public int turn_speed = 5;
	[Signal] delegate void Change_Health(int health);

    public RayCast3D rayCast;
    public int health = 100;
    public PackedScene explosion;

    public override void _Ready()
    {
        healthbar = GetNode<Node3D>("Healthbar");
        player_mesh = GetParent().GetParent().GetNode<Node3D>("taxi/Node3D");
        ball = GetParent().GetNode<RigidBody3D>("Ball");
        agent = GetNode<NavigationAgent3D>("root/NavigationAgent3D");
        navigation = GetParent().GetParent().GetNode<Navigation>("Navigation");
        agent.SetNavigation(navigation);
        update_path_timer = new Timer();
        AddChild(update_path_timer);
        update_path_timer.OneShot = true;
        update_path_timer.Start(0.1f);
        rayCast = GetNode<RayCast3D>("RayCast3D");
        character = GetNode<Node3D>("root");
        explosion = GD.Load<PackedScene>("res://Scenes/Explosion.tscn");
		Connect("Change_Health", new Callable(healthbar, nameof(Change_Health)));
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _PhysicsProcess(float delta)
    {
        //Align mesh
        GlobalTranslation = ball.GlobalTranslation + sphere_offset;
        var distance = (player_mesh.GlobalTransform.origin - GlobalTransform.origin).Length();
		if(distance > 2)
		{ 	
			
			speed_input = 1.5f;
		}
		else
		{
			speed_input = 0;
		}
		speed_input = Mathf.Lerp(speed_input , speed_input*acceleration , delta * 25);
		ball.AddConstantCentralForce(-GlobalTransform.basis.z * speed_input);
        
        // GlobalTranslation = GlobalTranslation.LinearInterpolate(player_mesh.GlobalTransform.origin, 0.07f);
    
    }
    public override void _Process(float delta)
    {
        if(update_path_timer.TimeLeft == 0)
        {
            LookAt(player_mesh.GlobalTranslation, Vector3.Up);
            update_path_timer.Start(0.35f);
        }
        
        character.Rotation = new Vector3(0, Mathf.DegToRad(180), 0);
    }
    public float Calculate_Angle(Vector3 direction)
	{
		// Calculate angle
		var angle = -GlobalTransform.basis.z.SignedAngleTo(direction , Vector3.Up);
		angle = Mathf.RadToDeg(angle);
		return angle;
	}
    public void Calculate_Health()
	{
		health -= 20;
		if(health <= 0)
		{
			health = 0;
			var explosion_instance = explosion.Instance() as Node3D;
			GetTree().Root.AddChild(explosion_instance);
			explosion_instance.GlobalTranslation = ball.GlobalTranslation;
			GetParent().QueueFree();
		}
		EmitSignal("Change_Health" ,health);
	}
    
//     public Transform Alignwithsurface(Transform xform ,Vector3 new_y)
// 	{
// 		xform.basis.y = new_y;
// 		xform.basis.x = -xform.basis.z.Cross(new_y);
// 		xform.basis = xform.basis.Orthonormalized();
// 		return xform;
// 	}
}
