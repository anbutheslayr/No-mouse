@tool
extends Node3D
class_name Minigun
@onready var cylinder: MeshInstance3D = $Cylinder
@export var target_speed: int = 5000
@export var speed: int = 0
@onready var tracer_pool: ObjectPool = $Tracer_pool
@onready var end_pos: Node3D = $Cylinder/RayCast3D/endpos
var cur_speed: int = 0
@onready var raycast: RayCast3D = $Cylinder/RayCast3D
func _process(delta: float) -> void:
	if cylinder:
		speed = lerp(speed, target_speed, delta)
		cur_speed = lerp(cur_speed, speed, delta)
		cylinder.rotation_degrees.x -=  cur_speed* delta
# func _ready() -> void:
# 	tracer = tracer_pool.get_instance() as Node3D
# 	add_child(tracer)
# 	tracer.global_transform.origin = raycast.global_transform.origin
# 	print(tracer.global_transform.origin)
# 	tween_check()
func _physics_process(delta: float) -> void:
	# tween_check()
	if cur_speed > 1500:
		var end_position: Vector3
		if raycast.is_colliding():
			end_position = raycast.get_collision_point()
		else :
			end_position = end_pos.global_transform.origin
		spawn_bullet_tracer(raycast.global_position,end_position)
func spawn_bullet_tracer(start_pos: Vector3, target_position: Vector3):
	# var tracer = TracerPool.get_instance() as Node3D
	var tracer = tracer_pool.get_instance() as Node3D
	tracer.global_position = start_pos
	tracer.look_at(target_position, Vector3.UP)

	# Create and configure the tween
	var tween = create_tween()
	tween.tween_property(tracer, "global_position", target_position, (tracer.global_position.distance_to(target_position) / tracer.global_position.distance_to(end_pos.global_position))*.18)
	tween.connect("finished", Callable(self,"_on_tween_finished").bind(tracer))

func _on_tween_finished(tracer: Node3D):
	# TracerPool.return_instance(tracer)
	tracer_pool.return_instance(tracer)
	

# func tween_check():
# 	var tween = create_tween()
# 	tracer.global_transform.origin = raycast.global_transform.origin
# 	tween.tween_property(tracer, "global_position", raycast.global_transform.origin + to_global(raycast.target_position), .2)
# 	tween.connect("finished", Callable(self,"tween_check"))



	
