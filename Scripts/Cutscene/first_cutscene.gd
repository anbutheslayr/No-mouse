extends Node3D
@onready var player_pcam : PhantomCamera3D = $PhantomCamera3D2
@onready var enemy_pcam : PhantomCamera3D = $PhantomCamera3D
@onready var third_pcam : PhantomCamera3D = $PhantomCamera3D3
@onready var follow_pcam : PhantomCamera3D = $PhantomCamera3D4
@onready var bomb : PackedScene = preload("res://Scenes/Bomb.tscn")
@onready var bomb_start_pos : Node3D = $bomb_start_pos
@onready var bomb_target : Node3D = $Bombtarg
@onready var main_cam : Camera3D = $Camera3D
@onready var player_nav_target : Node3D = $player_nav_target
@onready var cutscn_nav_target : Node3D = $cutscene_nav_path
func _ready():
	Dialogic.signal_event.connect(on_dialogic_event)
	Dialogic.preload_timeline("res://Dialogic/Characters/First cutscene.dtl")
	Dialogic.start("res://Dialogic/Characters/First cutscene.dtl")
	main_cam.set_process(false)
	main_cam.set_physics_process(false)
	Global.in_cutscene = true



func switch_pcam():
	enemy_pcam.set_priority((enemy_pcam.get_priority() + 1)%2)
	player_pcam.set_priority((player_pcam.get_priority() + 1)%2)
	

func on_dialogic_event(argument: String):
	if argument == "switch_pcam":
		switch_pcam()
	if argument == "delete_pcams":
		player_pcam.queue_free()
		enemy_pcam.queue_free()
	if argument == "switch_to_3rd":
		switch_to_3rd()

func switch_to_3rd():
	player_pcam.set_priority(0)
	enemy_pcam.set_priority(0)
	third_pcam.set_priority(1)
	await get_tree().create_timer(1).timeout
	throw_bomb()

func throw_bomb():
	var bomb_instance1 = bomb.instantiate() as RigidBody3D
	add_child(bomb_instance1)
	bomb_instance1.global_position = bomb_start_pos.global_position
	bomb_instance1.apply_central_impulse((bomb_target.global_position - bomb_instance1.global_position).normalized() * 15)
	await get_tree().create_timer(1.5).timeout
	Global.player_nav_targ = cutscn_nav_target.global_position
	Global.use_nav_for_player = true
	third_pcam.set_priority(0)
	follow_pcam.set_priority(1)
	await get_tree().create_timer(5).timeout
	delete_pcams()
	Global.use_nav_for_player = false

func delete_pcams():
	player_pcam.queue_free()
	enemy_pcam.queue_free()
	third_pcam.queue_free()
	follow_pcam.queue_free()
	main_cam.set_process(true)
	main_cam.set_physics_process(true)
	Dialogic.signal_event.disconnect(on_dialogic_event)
	Dialogic.end_timeline()
	Global.in_cutscene = false
	
