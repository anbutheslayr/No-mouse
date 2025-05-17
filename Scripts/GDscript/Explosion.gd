extends Node3D




@onready var area : Area3D = get_node("Area3D")
@onready var cam : Camera3D = get_tree().get_root().get_camera_3d()
@onready var popup : PackedScene = preload("res://Interface/Popup text.tscn")
@onready var player : RigidBody3D = get_tree().get_root().get_node("World/taxi/Ball")

func on_body_entered(body : Node):
	if body is RigidBody3D:
		var ball = body as RigidBody3D
		ball.apply_central_impulse(((ball.global_position - global_position).normalized()+Vector3(0,0.5,0))*40)
		cam.add_trauma(0.9)

	if body is RigidBody3D and body.is_in_group("Enemy"):
		body.get_parent().calculate_health(5)

	elif body.is_in_group("Runnable"):
		body.get_parent().get_parent().get_parent().calculate_health()
		body.get_parent().get_parent().get_parent().calculate_health()
		body.get_parent().get_parent().get_parent().calculate_health()

	var e = popup.instantiate() as Node3D
	get_tree().root.add_child(e)
	e.global_position = global_position
	e.play_anim("Boom!",20,3,global_position + Vector3(0,1.5,0),2)
	if global_position.distance_to(player.global_position) < 12:
		Engine.time_scale = 0.15
		await (get_tree().create_timer(0.15)).timeout
		Engine.time_scale = 1
