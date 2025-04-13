extends Node3D

@export var close_miss_bonus: int = 10
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
@export var accelerate_button_path: NodePath
@export var brake_path: NodePath
@export var left_path: NodePath
@export var right_path: NodePath
@export var damage_multiplier: float = 1.0
@export var health: float = 100.0
@export var jump_ht: float = 2.5
@export var drift_multiplier: int = 1
@export var im: int = 0
@export var cam_pos_path: NodePath
@export var fl_path: NodePath
@export var fr_path: NodePath
@export var bl_path: NodePath
@export var health_bar_path: NodePath

var expl = preload("res://Scenes/Explosion.tscn")
@onready var popuptext : PackedScene = preload("res://Interface/Popup text.tscn")
@onready var ball: RigidBody3D = get_node(ball_path)
@onready var car_mesh: MeshInstance3D = get_node(car_mesh_path)
@onready var left_wheel: MeshInstance3D = get_node(left_wheel_path)
@onready var right_wheel: MeshInstance3D = get_node(right_wheel_path)
@onready var car_mesh_body: MeshInstance3D = get_node(car_mesh_body_path)
@onready var accelerate_button: TouchScreenButton = get_node(accelerate_button_path)
@onready var brake : TouchScreenButton = get_node(brake_path)
@onready var left : TouchScreenButton = get_node(left_path)
@onready var right : TouchScreenButton = get_node(right_path)
@onready var cam_pos: Node3D = get_node(cam_pos_path)
@onready var fl: RayCast3D = get_node(fl_path)
@onready var fr: RayCast3D = get_node(fr_path)
@onready var bl: RayCast3D = get_node(bl_path)
@onready var cam : Camera3D = get_parent().get_node("Camera3D")
@onready var reso : Resolution = preload("user://Int/Res.tres")
@onready var b_l : CPUParticles3D
@onready var b_r : CPUParticles3D
@onready var audio_stream_player : AudioStreamPlayer = get_node("Ball/Oncollision")
@onready var drift : AudioStreamPlayer3D = get_node("Node3D/Drift")
@onready var hit_animation : AnimationPlayer = get_node("Hit Anim")
@onready var interface : Control = $Interface
@onready var health_bar_3d : Node3D = get_node(health_bar_path)
@onready var grav = ProjectSettings.get_setting("physics/3d/default_gravity")
var speed_input: float = 0.0
var steering_input: float = 0.0
var col_time: float = 0.0
var col: bool = false
var rc_iscol

func _ready():
	var e = load("res://Scenes/Explosion.tscn")
	var i = e.instantiate() as Node3D
	get_tree().root.call_deferred("add_child",i)
	i.global_position = ball.global_position
	part_change()

func _physics_process(_delta: float) -> void:
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
		ball.apply_central_force(-car_mesh.global_transform.basis.z*speed_input + add)

	

func jump():
	ball.linear_velocity.y = jump_ht*40
	cam.add_trauma(.8)

func _process(delta: float) -> void:
	# screenshot
	if Input.is_action_just_pressed("Save"):
		var image = get_viewport().get_texture().get_data()
		image.flip_y()
		image.save_png("D:/Godot export/SS/World1/" + str(im) + ".png")
		im += 1
	if!(fr.is_colliding() or fl.is_colliding() or bl.is_colliding()):
		return
	
	# Acceleration
	speed_input = 0
	speed_input = Input.get_axis( "Down","Up") * acceleration
	# print(speed_input)
	if accelerate_button.is_pressed():
		speed_input = acceleration
	elif brake.is_pressed():
		speed_input = -acceleration
	#Steering 
	steering_input = 0
	steering_input = Input.get_axis("Right", "Left") * deg_to_rad(steering)
	# print(steering_input) 
	if left.is_pressed():
		steering_input = deg_to_rad(steering)
	elif right.is_pressed():
		steering_input = -deg_to_rad(steering)
	# smoke
	var ball_vel = ball.linear_velocity
	var car_forward = car_mesh.global_transform.basis.z.normalized()
	var dot_pr = ball_vel.normalized().dot(car_forward)

	if bl.is_colliding() and ball_vel.length() > 13 and dot_pr > -.85 and dot_pr < 0:
		b_l.emitting = true
		b_r.emitting = true
		var points = ball_vel.length()/60*(1-dot_pr)*drift_multiplier
		interface.add_drift_points(points)
		if(!drift.playing):
			drift.playing = true
	else:
		b_l.emitting = false
		b_r.emitting = false
		drift.playing = false
	if col:
		col_time += delta
	else:
		col_time = 0
	if col_time >= .1:
		health -= delta*2
		health_bar_3d.change_health(health)
	# turning wheels
	
	left_wheel.rotation.y = PI + steering_input
	right_wheel.rotation.y = steering_input
	
	if b_l.emitting:
		left_wheel.rotation.y = -(PI + steering_input)
		right_wheel.rotation.y = -(steering_input - .3)

	if ball.linear_velocity.length() > turn_stop_limit:
		if speed_input < 0:
			steering_input = -steering_input
		# turning mesh
		var new_basis : Basis= car_mesh.global_transform.basis.rotated(car_mesh.global_transform.basis.y, steering_input)
		car_mesh.global_transform.basis = car_mesh.global_transform.basis.orthonormalized().slerp(new_basis.orthonormalized(), delta * turn_speed)
		car_mesh.global_transform = car_mesh.global_transform.orthonormalized()
		# applying tilt
		var t =-steering_input*ball.linear_velocity.length()/tilt
		car_mesh_body.rotation.z = lerp(car_mesh_body.rotation.z, t, delta * 10)

	# align with surface
	if Engine.time_scale > 0.5:
		var xform = align_with_surface(car_mesh.global_transform)
		car_mesh.global_transform = car_mesh.global_transform.interpolate_with(xform,.5)
	

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

func on_collision(body : Node):
	if body is RigidBody3D:
		var col_body = body as RigidBody3D
		var rel_vel = col_body.linear_velocity - ball.linear_velocity
		var imp_mag = rel_vel.length()
		# Apply damage
		var damage = calculate_damage(imp_mag)
		apply_damage(damage, body)
	if body.is_in_group("Obstacle") and ball.linear_velocity.length() > 6:
		audio_stream_player.play()
		cam.add_trauma(0.4)

func on_body_exit(body : Node):
	if body is RigidBody3D:
		close_miss(body)

func calculate_damage(imp_mag):
	var damage = round(imp_mag * damage_multiplier)
	if damage <= 3:
		damage = 0
	else:
		audio_stream_player.play()
		cam.add_trauma(0.5)
	return damage

func apply_damage(damage, body):
	health -= damage
	if damage != 0:
		hit_animation.play("Hit")
		var d = popuptext.instantiate() as Node3D
		get_tree().root.add_child(d)
		(d as Popuptext).play_anim(str(damage), 20, 3, car_mesh.global_transform.origin + Vector3(0, 2, 0), 1)
	if health <= 0:
		visible = false
		var e = expl.instantiate() as Node3D
		get_tree().root.add_child(e)
		e.global_position = ball.global_position
	health_bar_3d.change_health(health,true)
	body.get_parent().calculate_health(damage)

func revive_car():
	health = 100
	visible = true
	interface.show()
	health_bar_3d.change_health(health,true)

func enable_col():
	col = true

func disable_col():
	col = false

func close_miss(body : RigidBody3D):
	if body.linear_velocity.length() > 27:
		var p = popuptext.instantiate() as Node3D
		get_tree().root.add_child(p)
		p.play_anim("Close miss +10", 20, 3, car_mesh.global_transform.origin + Vector3(0, 2, 0), 1)
		health += close_miss_bonus
		health = clamp(health, 0, 100)
		health_bar_3d.change_health(health,false)

		
func part_change():
	if reso.cur_world == 1:
		b_l = get_node(b_l_particles)
		b_r = get_node(b_r_particles)
	elif reso.cur_world == 2:
		b_l = get_node(b_l2_particles)
		b_r = get_node(b_r2_particles)
		
