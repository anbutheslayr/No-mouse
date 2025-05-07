extends Gun_state
class_name MachineGunState
# This is the state for the machine gun weapon
# It handles the firing and reloading of the machine gun
# It also handles the input and animation for the machine gun

func state_enter():
	pass

func state_exit():
	pass


@export var raycastpath : NodePath
@export var animpath : NodePath
@export var gun_path : NodePath
@export var marker_path : NodePath
@export var ammo_text_path : NodePath
@export var rocket_launcher_path : NodePath
@export var rocket_button_path : NodePath
@export var audio_player_path : NodePath
@export var gun_damage : int = 5
@export var aim_speed : float = .25
@export var min_range : float = 2
@export var ammo : int
@export var start_magazines : int
@export var rocket_ammo : int = 15

@onready var raycast : RayCast3D = get_node(raycastpath)
@onready var anim : AnimationPlayer = get_node(animpath)
@onready var gun : MeshInstance3D= get_node(gun_path)
var decal : PackedScene = preload("res://Scenes/Decal.tscn")
@onready var audiostreamplayer : AudioStreamPlayer = get_node(audio_player_path)
@onready var  marker : Node3D = get_node(marker_path)
@onready var ammo_text : RichTextLabel = get_node(ammo_text_path)
var particle : PackedScene = preload("res://Scenes/Particles.tscn")
@onready var rocket_launcher : Node3D = get_node(rocket_launcher_path)
var roc_ammo : PackedScene = preload("res://Assets/Models/Guns/Rocket Ammo.tscn")
# @onready var rocket_button : TextureButton = get_node(rocket_button_path)
@onready var popuptext : PackedScene = preload("res://Interface/Popup text.tscn")
@onready var weapon_state_machine : WeaponStateMachine = get_parent()
var random
var cur_ammo
var cur_magazines
var rock_timer
var cur_gun : int = 1
var entered = false
var launch = false
var direction    

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
	
	


func state_process(delta):
	pass
	# Called to update the state logic (e.g., AI or game logic)  
	

func state_update(delta):
	direction = Vector3.ZERO
	if weapon_state_machine.closest_enemy != null:
		direction = weapon_state_machine.closest_enemy.global_position - raycast.global_position

	
	ammo_text.text = "       Ammo = " + str(cur_ammo) + "/" + str(ammo) + "(" + str(cur_magazines) + ") \n       " + str(DisplayServer.screen_get_size()) + "\n       FPS : " + str(Engine.get_frames_per_second()) + "\n       Enemies Alive : " + str(get_tree().get_nodes_in_group("Enemy").size())
	
	if weapon_state_machine.enemies.size() > 0 and cur_ammo > 0:
		if gun.global_position.distance_to(weapon_state_machine.closest_enemy.global_position) > min_range:
			var aimspd = aim_speed
			if weapon_state_machine.closest_enemy.is_in_group("Runnable"):
				aimspd = .95
			gun.look_at(marker.global_position.lerp( raycast.global_position-direction, aimspd), Vector3.UP)
			
			anim.play("Shoot")
			
			
			
			
			
			
	elif cur_ammo <= 0 and cur_magazines > 0:
		# anim.play("Reload")
		# await anim.animation_finished
		cur_ammo = ammo
		cur_magazines -= 1
	else:
		state_changed.emit(self, "Machine_gun_descend")
	
func on_shoot():
	
	if raycast.is_colliding() and cur_ammo > 0:
		var a = particle.instantiate() as Node3D;
		var b = decal.instantiate() as Node3D;
		get_tree().root.get_node("World").add_child(a)
		raycast.get_collider().add_child(b)
		a.global_position = raycast.get_collision_point()
		b.global_position = raycast.get_collision_point()
		if raycast.get_collision_normal() != Vector3.UP:
			b.look_at(raycast.get_collision_point() + raycast.get_collision_normal(), Vector3.UP)
		if raycast.get_collider().is_in_group("Enemy_Body"):
			var enemy = raycast.get_collider().get_parent().get_parent().get_parent() as Node3D
			enemy.call("calculate_health" , gun_damage)
			var d = popuptext.instantiate()
			get_tree().root.get_node("World").add_child(d)
			(d as Popuptext).play_anim( "Hit" , 10 , 5 , raycast.get_collision_point() + Vector3(0,1,0),1)
			
		if raycast.get_collider().is_in_group("Runnable"):
			raycast.get_collider().get_parent().get_parent().get_parent().call("Calculate_Health")
		audiostreamplayer.play()
	elif cur_ammo <= 0:
		cur_ammo = 1
	cur_ammo -= 1
