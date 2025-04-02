extends RigidBody3D

@onready var player : RigidBody3D = get_parent().get_node("taxi/Ball")
@onready var capsule : MeshInstance3D = get_node("MeshInstance3D")
@onready var attack : AudioStreamPlayer = get_node("Attack")
@onready var camera : Camera3D = get_parent().get_node("Camera3D")
var raycast : RayCast3D
@export var follow_altitude : float = 10
@export var trauma_amount : float = 0.5
@export var avoidance_strength : float = 10
@export var follow_speed : float = 5
var target_pos
var paused : bool = false

func _ready():
	raycast = RayCast3D.new()
	add_child(raycast)
	raycast.enabled = true
	raycast.add_exception(self)
	raycast.add_exception(player)

func _process(_delta: float) -> void:
	target_pos = player.global_transform.origin + Vector3(0, follow_altitude, 0)
	raycast.target_position = target_pos - global_transform.origin
	raycast.force_raycast_update()
	if (global_transform.origin.distance_to(player.global_transform.origin) < 4):
		player.get_parent().call("enable_col")
		capsule.show()
		attack.playing = true
		camera.call("add_trauma" , trauma_amount)
	else:
		player.get_parent().call("disable_col")
		capsule.hide()
		attack.playing = false

func _physics_process(_delta: float) -> void:
	if (raycast.is_colliding()):
		var avoid_dir = raycast.get_collision_normal().cross(Vector3.UP).normalized()
		target_pos += avoid_dir * avoidance_strength
	if !paused:
		global_transform.origin = global_transform.origin.lerp(target_pos, follow_speed)

func pause():
	paused = true

func resume():
	paused = false
