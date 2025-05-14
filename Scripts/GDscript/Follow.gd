extends Camera3D

@export var lerp_speed: float = 3
@export var target_path: NodePath
@export var offset: Vector3 
@export var trauma_red_rate = 1
@export var fastnoiselite : Noise
@export var noise_speed : int = 50
@export var max_x : int = 10
@export var max_y : int = 10
@export var max_z : int  = 5
@export var col_lerp_speed : int = 3


@onready var target : MeshInstance3D = get_node(target_path)
@onready var rayp : Node3D = target.get_node("Raycol")
# @onready var gun : Gun = target.get_node("body/MachineGun")
var raycast : RayCast3D
var trauma = 0
var time=0

func _ready() -> void:
	raycast = RayCast3D.new()
	add_child(raycast)
	raycast.enabled = true
	raycast.collide_with_areas = false
	raycast.collide_with_bodies = true




func _physics_process(delta: float) -> void:
	var targetpos : Transform3D = target.global_transform.translated_local(offset)
	targetpos.origin.y = max(targetpos.origin.y, target.global_transform.origin.y + offset.y)
	raycast.global_position = rayp.global_position
	var dir = global_position - raycast.global_position
	raycast.target_position = dir
	raycast.force_raycast_update()
<<<<<<< HEAD
	if(raycast.get_collider() is StaticBody3D):
		global_position = global_position.lerp(Vector3(targetpos.origin.x, 25, targetpos.origin.z),col_lerp_speed*delta)
	else:
		global_position = global_position.lerp(targetpos.origin, lerp_speed*delta)
=======
	# if(raycast.get_collider() is StaticBody3D):
	# 	global_position = global_position.lerp(Vector3(targetpos.origin.x, 25, targetpos.origin.z),col_lerp_speed*delta)
	# else:
	global_position = global_position.lerp(targetpos.origin, lerp_speed*delta)
>>>>>>> d22ba05983e933ea4e4e4164f1b84b644234d300
	look_at(target.global_transform.origin, Vector3.UP)
	trauma = max(trauma - trauma_red_rate*delta, 0)
	time += delta
	rotation_degrees = Vector3(rotation_degrees.x + get_noise_from_seed(0)*max_x*get_shake_intensity(),
	rotation_degrees.y + get_noise_from_seed(1)*max_y*get_shake_intensity(),
	rotation_degrees.z + get_noise_from_seed(2)*max_z*get_shake_intensity())
	
func add_trauma(amount):
	trauma = clamp(trauma + amount, 0, 1)


func get_shake_intensity():
	return trauma*trauma

func get_noise_from_seed(sed):
	fastnoiselite.seed = sed
	return fastnoiselite.get_noise_1d(time * noise_speed)
