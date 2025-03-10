extends Node


func on_collision(node : Node):
    if node.is_in_group("Ball"):
        node.get_parent().get_node("Node3D/body/MachineGun").call("Add_ammo")
        queue_free()
    
