extends RigidBody3D
@onready var explosion : PackedScene = preload("res://Scenes/Explosion.tscn")

func on_area_entered(area: Area3D) -> void:
	if area.is_in_group("Enemy_Body"):
		var explosion_instance = explosion.instantiate() as Node3D
		get_tree().root.add_child(explosion_instance)
		explosion_instance.global_position = global_position
		queue_free()
	
	