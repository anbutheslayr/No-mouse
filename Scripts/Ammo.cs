using Godot;
using System;

public class Ammo : Spatial
{
    public void On_collision(Node node)
    {
        if(node.IsInGroup("Ball"))
        {
            node.GetParent().GetNode("Spatial/body/MachineGun").Call("Add_ammo");
            QueueFree();
        }
    }
}
