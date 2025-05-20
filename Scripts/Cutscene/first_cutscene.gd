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
@onready var level_end : Node3D = $level_end
@onready var final_pcam : PhantomCamera3D = $PhantomCamera3D5
@onready var BgAudioPlayer : AudioStreamPlayer = $bg
@onready var final_shot_pcam : PhantomCamera3D = $PhantomCamera3D6
@onready var anim : AnimationPlayer = $AnimationPlayer
@onready var end_label : Label = $Control/Label
func _ready():
	Global.in_cutscene = true
	await get_tree().create_timer(.2).timeout
	Dialogic.signal_event.connect(on_dialogic_event)
	Dialogic.preload_timeline("res://Dialogic/Characters/First cutscene.dtl")
	Dialogic.start("res://Dialogic/Characters/First cutscene.dtl")
	main_cam.set_process(false)
	main_cam.set_physics_process(false)
	Global.reso.weapons.set("Machine_gun",0) 
	Global.reso.weapons.set("Minigun",0)
	main_cam.global_position = player_pcam.global_position
	Global.max_nav_angle = 7
	Global.turn_lerp_speed = 30


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
	final_pcam.set_priority(0)
	third_pcam.set_priority(1)
	await get_tree().create_timer(1).timeout
	throw_bomb()

func throw_bomb():
	var bomb_instance1 = bomb.instantiate() as RigidBody3D
	add_child(bomb_instance1)
	bomb_instance1.global_position = bomb_start_pos.global_position
	bomb_instance1.apply_central_impulse((bomb_target.global_position - bomb_instance1.global_position).normalized() * 18)
	await get_tree().create_timer(1.5).timeout
	Global.player_nav_targ = cutscn_nav_target.global_position
	Global.use_nav_for_player = true
	Global.player_nav_speed = 60
	third_pcam.set_priority(0)
	follow_pcam.set_priority(1)
	# BgAudioPlayer.playing = true
	print("call play")
	await get_tree().create_timer(3).timeout
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
	Global.show_obj = true
	Global.cur_objective = "ESCAPE"
	Global.player_nav_targ = level_end.global_position
	main_cam.get_child(0).queue_free()
	BgAudioPlayer.play()
	
func levelend(body : Node3D):
	if body.is_in_group("Ball") and body is RigidBody3D:
		main_cam.add_child(PhantomCameraHost.new())
		Global.use_nav_for_player = true
		Global.in_cutscene = true
		final_pcam.set_priority(1)
		Global.player_nav_speed = 50
		Engine.time_scale = lerp(Engine.time_scale, .8, 0.5)
		Global.max_nav_angle = 10
		Global.turn_lerp_speed = 7
		BgAudioPlayer.stop()

func final_shot(body : Node3D):
	if body.is_in_group("Ball") and body is RigidBody3D:
		final_pcam.set_priority(0)
		final_shot_pcam.set_priority(1)
		Engine.time_scale = lerp(Engine.time_scale, .000000001, 0.9)
		await get_tree().create_timer(.3).timeout
		Engine.time_scale = 1
		match Global.reso.difficulty:
			0:
				end_label.text = "NOOB LOL \n try beating at medium diff loser"
			1:
				end_label.text = "MID LOL \n try beating at hard diff loser"
			2:
				end_label.text = "that's it for now nigga"
		anim.play("end")

		

		


func next_level():
	Engine.time_scale = 1
	Global.max_nav_angle = 10
	Global.turn_lerp_speed = 10
	Global.in_cutscene = true
	LoadingScreen.load_scene("res://Scenes/Main_menu.tscn")
	# Global.show_obj = true
	# Global.cur_objective = "ESCAPE"
	# Global.player_nav_targ = level_end.global_position
	# main_cam.get_child(0).queue_free()