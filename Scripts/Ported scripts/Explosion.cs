using Godot;
using System;

public partial class Explosion : Node3D
{
    public Area3D area;
    public Camera3D cam; 
    public PackedScene popupttext;
    public ColorRect bw;
    public RigidBody3D player;
    public override void _Ready()
    {
        area = GetNode<Area3D>("Area3D");
        cam = GetTree().Root.GetNode<Camera3D>("World/Camera3D");
        popupttext = GD.Load<PackedScene>("res://Interface/Popup text.tscn");
        player = GetTree().Root.GetNode<RigidBody3D>("World/taxi/Ball");
    }
    public async void On_body_entered(Node body)
    {
        if (body is RigidBody3D)
        {
            var ball = body as RigidBody3D;
            ball.ApplyImpulse(ball.GlobalTranslation - GlobalTranslation , ((ball.GlobalTranslation - GlobalTranslation).Normalized()+new Vector3(0,0.5f,0))*35);
            cam.Call("Add_trauma",0.9f);
        }        
        if (body is RigidBody3D && body.IsInGroup("Enemy"))
        {
            body.GetParent().Call("Calculate_Health", 5);
        }
        else if (body.IsInGroup("Runnable"))
        {
            body.GetParent().GetParent().GetParent().Call("Calculate_Health");
            body.GetParent().GetParent().GetParent().Call("Calculate_Health");
            body.GetParent().GetParent().GetParent().Call("Calculate_Health");
        }
        var e = popupttext.Instance() as Node3D;
        GetTree().Root.AddChild(e);
        e.GlobalTranslation = GlobalTranslation;
        (e as Popuptext).PlayAnim("Boom!",20,3,GlobalTranslation + new Vector3(0,1.5f,0),2);
        if(GlobalTranslation.DistanceTo(player.GlobalTranslation) < 12)
        {
           Engine.TimeScale = 0.15f;
            GD.Print("pause");
            await ToSignal(GetTree().CreateTimer(0.15f), "timeout");
            Engine.TimeScale = 1; 
        }
        
    }
}
