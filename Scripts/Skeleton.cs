using Godot;
using System;

public class Skeleton : Spatial
{
    public Spatial healthbar;
    public Spatial player_mesh;
    public Spatial character;
    public RigidBody ball;
    public Vector3 sphere_offset = new Vector3(0, -0.9f, 0);
    public NavigationAgent agent;
    [Export]public float speed_input;
    public Vector3 nexpos;
    public Navigation navigation;
    public Timer update_path_timer;
    public float steering_input;
    [Export] public float steering = 60;
    [Export] public int acceleration = 25;
    [Export] public int turn_speed = 5;
    public RayCast rayCast;

    public override void _Ready()
    {
        healthbar = GetNode<Spatial>("Healthbar");
        player_mesh = GetParent().GetParent().GetNode<Spatial>("taxi/Spatial");
        ball = GetParent().GetNode<RigidBody>("Ball");
        agent = GetNode<NavigationAgent>("root/NavigationAgent");
        navigation = GetParent().GetParent().GetNode<Navigation>("Navigation");
        agent.SetNavigation(navigation);
        update_path_timer = new Timer();
        AddChild(update_path_timer);
        update_path_timer.OneShot = true;
        update_path_timer.Start(0.1f);
        rayCast = GetNode<RayCast>("RayCast");
        character = GetNode<Spatial>("root");
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
		ball.AddCentralForce(-GlobalTransform.basis.z * speed_input);
        
        // GlobalTranslation = GlobalTranslation.LinearInterpolate(player_mesh.GlobalTransform.origin, 0.07f);
    
    }
    public override void _Process(float delta)
    {
        if(update_path_timer.TimeLeft == 0)
        {
            LookAt(player_mesh.GlobalTranslation, Vector3.Up);
            update_path_timer.Start(0.35f);
        }
        
        character.Rotation = new Vector3(0, Mathf.Deg2Rad(180), 0);
    }
    public float Calculate_Angle(Vector3 direction)
	{
		// Calculate angle
		var angle = -GlobalTransform.basis.z.SignedAngleTo(direction , Vector3.Up);
		angle = Mathf.Rad2Deg(angle);
		return angle;
	}
    
//     public Transform Alignwithsurface(Transform xform ,Vector3 new_y)
// 	{
// 		xform.basis.y = new_y;
// 		xform.basis.x = -xform.basis.z.Cross(new_y);
// 		xform.basis = xform.basis.Orthonormalized();
// 		return xform;
// 	}
}
