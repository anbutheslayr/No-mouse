extends Node3D

@export var ball_path: NodePath
@export var car_mesh_body_path: NodePath
@export var car_mesh_path: NodePath
@export var sphere_offset: Vector3 = Vector3(0, -1, 0)
@export var acceleration: float = 50.0
@export var steering: float = 50.0
@export var turn_speed: float = 5.0
@export var turn_stop_limit: float = 0.75
@export var tilt: float = 35.0
@export var left_wheel_path: NodePath
@export var right_wheel_path: NodePath
@export var b_l_particles: NodePath
@export var b_r_particles: NodePath
@export var b_l2_particles: NodePath
@export var b_r2_particles: NodePath
@export var health: float = 100.0
@export var jump_ht: float = 2.5
@export var fl_path: NodePath
@export var fr_path: NodePath
@export var bl_path: NodePath
@export var health_bar_path: NodePath
@export var nav_agent_path : NodePath
var expl = preload("res://Scenes/Explosion.tscn")
@onready var ball: RigidBody3D = get_node(ball_path)
@onready var car_mesh: MeshInstance3D = get_node(car_mesh_path)
@onready var left_wheel: MeshInstance3D = get_node(left_wheel_path)
@onready var right_wheel: MeshInstance3D = get_node(right_wheel_path)
@onready var car_mesh_body: MeshInstance3D = get_node(car_mesh_body_path)
@onready var fl: RayCast3D = get_node(fl_path)
@onready var fr: RayCast3D = get_node(fr_path)
@onready var bl: RayCast3D = get_node(bl_path)
@onready var b_l : CPUParticles3D
@onready var b_r : CPUParticles3D
@onready var drift : AudioStreamPlayer3D = get_node("Node3D/Drift")
@onready var health_bar_3d : Node3D = get_node(health_bar_path)
@onready var grav = ProjectSettings.get_setting("physics/3d/default_gravity")
@onready var reso : Resolution = preload("user://Int/Res.tres")
@onready var nav_agent : NavigationAgent3D = get_node(nav_agent_path)
@onready var update_path_timer : Timer = Timer.new()
@onready var player_mesh : MeshInstance3D = get_parent().get_node("taxi/Node3D")
var wait_time 
var next_point : Vector3
var speed_input: float = 1
var steering_input= 0.0
var rc_iscol: bool

func _ready():
	part_change()
	set_difficulty()
	add_child(update_path_timer)
	update_path_timer.one_shot = true
	update_path_timer.wait_time = .1
	update_path_timer.start()

func set_difficulty():
	match reso.difficulty:
		0:
			wait_time = .1
			acceleration = 95
			turn_speed = 3
		1:
			wait_time = .1
			acceleration = 110
			turn_speed = 4
		2:
			wait_time = .1
			acceleration = 130
			turn_speed = 5

func part_change():
	if reso.cur_world == 1:
		b_l = get_node(b_l_particles)
		b_r = get_node(b_r_particles)
	elif reso.cur_world == 2:
		b_l = get_node(b_l2_particles)
		b_r = get_node(b_r2_particles)

func  _physics_process(_delta: float) -> void:
	rc_iscol = (fr.is_colliding() or fl.is_colliding() or bl.is_colliding())
	# Align mesh with sphere
	
	car_mesh.transform.origin = ball.transform.origin + sphere_offset  
	# Accelerate
	var add = Vector3.ZERO
	# if car_mesh.rotation.x>0 :
	# 	add = -car_mesh.global_transform.basis.z*speed_input*sin(car_mesh.rotation.x)*ball.mass*grav
	# else:
	# 	add = Vector3.ZERO
	if rc_iscol:
		ball.apply_central_force(-car_mesh.global_transform.basis.z*acceleration+ add)

func align_with_surface(xform: Transform3D) -> Transform3D:

	var front_left_col = fl.get_collision_point() if fl.is_colliding() else fl.global_position
	var back_left_col = bl.get_collision_point() if bl.is_colliding() else bl.global_position
	var front_right_col = fr.get_collision_point() if fr.is_colliding() else fr.global_position

	var side_vector = (front_right_col - front_left_col).normalized()
	var forward_vector = (back_left_col - front_left_col).normalized()  

	var new_y = forward_vector.cross(side_vector).normalized()
	xform.basis.y = new_y
	xform.basis.x = -xform.basis.z.cross(new_y)
	return xform.orthonormalized()

func calculate_health(damage):
	health -=damage
	if health < 0:
		get_parent().get_node("Camera3D").add_trauma(0.8)
		player_mesh.get_parent().get_node("Interface").killed()
		health = 0
		var exp_inst = expl.instantiate() as Node3D
		exp_inst.global_position = ball.global_position
		get_tree().root.add_child(exp_inst)
		queue_free()
	health_bar_3d.change_health(health)

func _process(delta: float) -> void:
	if Engine.time_scale != 1:
		steering_input = 0.0
	# AI
	if update_path_timer.time_left ==0:
		nav_agent.target_position = player_mesh.global_position
		update_path_timer.start(wait_time)
		next_point = nav_agent.get_next_path_position()
	# var angle = rad_to_deg(-car_mesh.global_transform.basis.z.signed_angle_to(next_point,Vector3.UP))
	var angle = rad_to_deg(-car_mesh.global_transform.basis.z.signed_angle_to(next_point - ball.global_position,Vector3.UP))

	if angle > 20:
		steering_input = lerp(steering_input,deg_to_rad(steering),delta*10)
	elif angle<-20:
		steering_input = lerp(steering_input,-deg_to_rad(steering),delta*10)
	 
	print(angle)

	# print(steering_input)
	# turning wheels
	left_wheel.rotation.y = lerp(left_wheel.rotation.y,PI + steering_input,delta*40)
	right_wheel.rotation.y = lerp(right_wheel.rotation.y,steering_input - .3,delta*40)

	if b_l.emitting:
		left_wheel.rotation.y = -(PI + steering_input)
		right_wheel.rotation.y = -(steering_input - .3)
	if ball.linear_velocity.length() > turn_stop_limit:
		# turning mesh
		var new_basis : Basis= car_mesh.global_transform.basis.rotated(car_mesh.global_transform.basis.y, steering_input)
		car_mesh.global_transform.basis = car_mesh.global_transform.basis.orthonormalized().slerp(new_basis.orthonormalized(), delta * turn_speed)
		car_mesh.global_transform = car_mesh.global_transform.orthonormalized()
		# # applying tilt
		# var t =-steering_input*ball.linear_velocity.length()/tilt
		# car_mesh_body.rotation.z = lerp(car_mesh_body.rotation.z, t, delta * 10)


	# speed_input = lerp(speed_input,speed_input*acceleration,delta*25)
	if ball.global_position.distance_to(player_mesh.global_position)>500:
		ball.global_position = Vector3(0,5,0)
	# align with surface
	
	var xform = align_with_surface(car_mesh.global_transform)
	car_mesh.global_transform = car_mesh.global_transform.interpolate_with(xform,.5)
