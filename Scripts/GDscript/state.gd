extends Node
class_name Gun_state


@export var raycastpath : NodePath
@export var animpath : NodePath
@export var gun_path : NodePath
@export var gun_damage : int = 5
@export var aim_speed : float = .25
@export var min_range : float = 2
@export var ammo : int
@export var start_magazines : int
@export var rocket_ammo : int = 15

@onready var raycast : RayCast3D = get_node(raycastpath)
@onready var anim : AnimationPlayer = get_parent().get_parent().get_node(animpath)
@onready var gun : MeshInstance3D= get_node(gun_path)
var decal : PackedScene = preload("res://Scenes/Decal.tscn")
@onready var audiostreamplayer : AudioStreamPlayer = get_node("AudioStreamPlayer")
@onready var  marker : Node3D = get_node("Gun/Marker")
@onready var ammo_text : RichTextLabel = get_parent().get_parent().get_parent().get_node("Interface/Ammo_text")
var particle : PackedScene = preload("res://Scenes/Particles.tscn")
@onready var player : Node3D = get_parent().get_parent().get_parent()
@onready var rocket_launcher : Node3D = get_parent().get_node("Rocket launcher")
var roc_ammo : PackedScene = preload("res://Assets/Models/Guns/Rocket Ammo.tscn")
@onready var rocket_button : TextureButton = get_parent().get_parent().get_parent().get_node("Interface/Rocket_button")
@onready var popuptext : PackedScene = preload("res://Interface/Popup text.tscn")
var random
var cur_ammo
var cur_magazines
var rock_timer
var enemies : Array 
var closest_enemy
var cur_gun : int = 1
var entered = false
var launch = false
var direction

signal state_changed
# Called when the state is entered
func state_enter():
	pass

# Called when the state is exited
func state_exit():
	pass

# Called to update the state logic (e.g., AI or game logic)
func state_update(delta):
	pass

# Called to process the state (e.g., physics or frame-specific logic)
func state_process(delta):
	pass

func  _ready() -> void:
	raycast.enabled = true
	cur_ammo = ammo
	cur_magazines = start_magazines
	rock_timer = Timer.new()
	add_child(rock_timer)
	rock_timer.wait_time = .2
	rock_timer.one_shot = true
	rock_timer.connect("timeout",Callable(self,"on_timeout"))
	rock_timer.start()


func _process(delta: float) -> void:
	direction = Vector3.ZERO
	ammo_text.text = "       Ammo = " + str(cur_ammo) + "/" + str(ammo) + "(" + str(cur_magazines) + ") \n       " + str(DisplayServer.screen_get_size()) + "\n       FPS : " + str(Engine.get_frames_per_second()) + "\n       Enemies Alive : " + str(get_tree().get_nodes_in_group("Enemy").size());
	closest_enemy = get_closest_enemy()
	if closest_enemy != null:
		direction = closest_enemy.global_position - raycast.global_position
	

func on_detection(body : Node):
	if body.is_in_group("Enemy") or body.is_in_group("Runnable"):
		enemies.append(body as Node3D)
	
func on_exit(body : Node):
	if body.is_in_group("Enemy") or body.is_in_group("Runnable"):
		enemies.erase((body as Node3D))





func get_closest_enemy() -> Node3D:
	var closest : Node3D = null
	var distance = 9999
	for i in enemies:
		var dist = i.global_position.distance_to(player.global_position)
		if dist < distance or closest == null:
			closest = i
			distance = dist
	return closest
