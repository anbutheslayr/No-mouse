extends Area3D
class_name RocketAmmo
var launch : bool = false
var direction : Vector3 
@onready var part : GPUParticles3D = $part/GPUParticles3D
@onready var marker : Node3D = $target
@onready var thrust : AudioStreamPlayer3D = $Thruster
@export var speed : float = 100
@export var rot_speed : float = 1
var target 
var expl = preload("res://Scenes/Explosion.tscn")





func launch_rocket():
	launch = true
	thrust.playing = true
	part.emitting = true

func _physics_process(delta: float) -> void:
	if launch:
		global_translate(global_transform.basis.z * delta * speed)
		target = get_closest_enemy()
		if target != null:
			direction = (target.global_position - global_position).normalized()
			marker.look_at(marker.global_position.lerp(global_position-direction, rot_speed*delta ), Vector3.UP)



func on_collision(body: Node3D):
	if launch:
		var explosion = expl.instantiate()
		explosion.global_position = global_position
		get_tree().current_scene.add_child(explosion)
		part.reparent(get_tree().current_scene)
		part.emitting = false
		queue_free()

		if body is RigidBody3D and body.is_in_group("Enemy"):
			body.get_parent().calculate_health(20)

		if body.is_in_group("runnable"):
			# body.get_parent().calculate_health(100)
			pass
		
			




func get_closest_enemy() -> Node3D:
	var closest : Node3D = null
	var distance = 9999
	for i in get_tree().get_nodes_in_group("enemy") + get_tree().get_nodes_in_group("runnable"):
		var dist = i.global_position.distance_to(global_position)
		if dist < distance or closest == null:
			closest = i
			distance = dist
	return closest