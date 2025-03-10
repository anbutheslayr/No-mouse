using Godot;
using System;

public partial class Ammo : Node3D
{
    public void On_collision(Node node)
    {
        if(node.IsInGroup("Ball"))
        {
            node.GetParent().GetNode("Node3D/body/MachineGun").Call("Add_ammo");
            QueueFree();
        }
    }
}
