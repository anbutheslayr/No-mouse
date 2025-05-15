@tool
extends RigidBody3D
func boing():
	apply_central_force((get_parent().get_node("Bombtarg") as Node3D).global_position.normalized()*500)
	