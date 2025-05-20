extends Area3D
@onready var enemy_scn: PackedScene = preload("res://Scenes/debug taxi.tscn")

func start_trap(area : Area3D):
	if area.is_in_group("Player_body"):
		var enemy_instance = enemy_scn.instantiate() as Node3D
		get_tree().current_scene.add_child(enemy_instance)
		enemy_instance.global_position = global_position
		print("trap")
		