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
@onready var update_path_timer : Timer 
var wait_time 
var speed_input: float = 1
var steering_input: float = 0.0
var rc_iscol

func _ready():
    part_change()
    set_difficulty()
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
    rc_iscol = (fr.is_colliding() and fl.is_colliding() and bl.is_colliding())
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








