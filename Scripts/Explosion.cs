using Godot;
using System;

public class Explosion : Spatial
{
    public Area area;
    public Camera cam; 
    public PackedScene popupttext;
    public ColorRect bw;
    public RigidBody player;
    public override void _Ready()
    {
        area = GetNode<Area>("Area");
        cam = GetTree().Root.GetNode<Camera>("World/Camera");
        popupttext = GD.Load<PackedScene>("res://Interface/Popup text.tscn");
        player = GetTree().Root.GetNode<RigidBody>("World/taxi/Ball");
    }
    public async void On_body_entered(Node body)
    {
        if (body is RigidBody)
        {
            var ball = body as RigidBody;
            ball.ApplyImpulse(ball.GlobalTranslation - GlobalTranslation , ((ball.GlobalTranslation - GlobalTranslation).Normalized()+new Vector3(0,0.5f,0))*35);
            cam.Call("Add_trauma",0.9f);
        }        
        if (body is RigidBody && body.IsInGroup("Enemy"))
        {
            body.GetParent().Call("Calculate_Health", 5);
        }
        else if (body.IsInGroup("Runnable"))
        {
            body.GetParent().GetParent().GetParent().Call("Calculate_Health");
            body.GetParent().GetParent().GetParent().Call("Calculate_Health");
            body.GetParent().GetParent().GetParent().Call("Calculate_Health");
        }
        var e = popupttext.Instance() as Spatial;
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
