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
@export var damage_multiplier: float = 1.0
@export var health: float = 100.0
@export var ramp_speed: float = 3.0
@export var jump_ht: float = 2.5
@export var drift_multiplier: int = 1
@export var im: int = 0
@export var cam_pos_path: NodePath
@export var fl_path: NodePath
@export var fr_path: NodePath
@export var bl_path: NodePath

@onready var ball: RigidBody3D = get_node(ball_path)
@onready var car_mesh: MeshInstance3D = get_node(car_mesh_path)
@onready var left_wheel: MeshInstance3D = get_node(left_wheel_path)
@onready var right_wheel: MeshInstance3D = get_node(right_wheel_path)
@onready var car_mesh_body: MeshInstance3D = get_node(car_mesh_body_path)
@onready var accelerate_button: TouchScreenButton = get_node(accelerate_button_path)
@onready var cam_pos: Node3D = get_node(cam_pos_path)
@onready var fl: RayCast3D = get_node(fl_path)
@onready var fr: RayCast3D = get_node(fr_path)
@onready var bl: RayCast3D = get_node(bl_path)

var speed_input: float = 0.0
var steering_input: float = 0.0
var is_on_ramp: bool = false
var col_time: float = 0.0
var col: bool = false

func _ready():
    pass

func _physics_process(delta: float):
    car_mesh.transform.origin = ball.transform.origin + sphere_offset
    
    speed_input = Input.get_action_strength("ui_up") - Input.get_action_strength("ui_down")
    if accelerate_button.is_pressed():
        speed_input = 1
    
    speed_input = lerp(speed_input, speed_input * acceleration, delta * 25)
    
    steering_input = Input.get_action_strength("ui_left") - Input.get_action_strength("ui_right")
    steering_input *= deg_to_rad(steering)
    
    if is_on_ramp:
        speed_input *= ramp_speed
    
    var add = -car_mesh.global_transform.basis.z * sin(car_mesh.rotation.x) * ball.mass * 3 if car_mesh.rotation.x > 0 else Vector3.ZERO
    ball.add_constant_central_force(-car_mesh.global_transform.basis.z * speed_input + add)
    
    if col:
        col_time += delta
    else:
        col_time = 0
    
    if col_time >= 0.1:
        health -= delta * 2

func jump():
    ball.linear_velocity.y = jump_ht * 40
    print("JUMP")

func align_with_surface(transform: Transform3D) -> Transform3D:
    var front_left_col = fl.get_collision_point() if fl.is_colliding() else fl.global_transform.origin
    var back_left_col = bl.get_collision_point() if bl.is_colliding() else bl.global_transform.origin
    var front_right_col = fr.get_collision_point() if fr.is_colliding() else fr.global_transform.origin
    
    var side_vector = (front_right_col - front_left_col).normalized()
    var forward_vector = (back_left_col - front_left_col).normalized()
    
    var new_y = forward_vector.cross(side_vector).normalized()
    transform.basis.y = new_y
    transform.basis.x = -transform.basis.z.cross(new_y)
    transform.basis = transform.basis.orthonormalized()
    return transform

func on_collision(body: Node):
    if body.is_in_group("Ramp"):
        is_on_ramp = true
    if body is RigidBody3D:
        var relative_velocity = body.linear_velocity - ball.linear_velocity
        var impact_magnitude = relative_velocity.length()
        var damage = calculate_damage(impact_magnitude)
        apply_damage(damage, body)

func apply_damage(damage: float, body: Node):
    health -= damage
    if health <= 0:
        visible = false

func calculate_damage(impact_magnitude: float) -> float:
    var damage = roundi(impact_magnitude * damage_multiplier)
    return damage if damage > 3 else 0

func enable_col():
    col = true

func disable_col():
    col = false
