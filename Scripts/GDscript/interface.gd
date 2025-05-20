extends Control

class_name interface

@onready var accelerate_button: TouchScreenButton = $Acceleration/Accelerate
@onready var brake : TouchScreenButton = $Acceleration/Brake
@onready var left : TouchScreenButton = $Steering/Left
@onready var right : TouchScreenButton = $Steering/Right
@onready var esc : TextureButton = $Esc
@onready var enemyspawner : Node3D = get_tree().get_nodes_in_group("Enemy_spawner")[0]
@onready var enemy_taxi_scene : PackedScene = ResourceLoader.load("Scenes/debug taxi.tscn")
@onready var timer : Timer = Timer.new()
@onready var cur_objective : Label = $Enemy_spawntext
@onready var pausemenu : Control = get_tree().get_nodes_in_group("Pause_menu")[0]
@onready var gun_switch : TouchScreenButton  = $Weapon_select
# @onready var plane : RigidBody3D = get_tree().get_nodes_in_group("Plane")[0]
@onready var drift_points : RichTextLabel = $Drift_points
@onready var change_world_timer : Timer = Timer.new()
# @onready var skeleton : PackedScene = Global.ResourceLoader.load("res://Assets/Models/World2/Scenes/character-skeleton.tscn")
@export var min_kills = 1
@onready var cur_enemies = 0
@onready var dead = false
@onready var won = false
@onready var spawntime =26
@onready var kill = 1
@onready var weapon_switch : TouchScreenButton = $Weapon_select
func _ready():
	timer.one_shot = true
	timer.connect("timeout", Callable.create(self,"change_world"))
	add_child(timer)
	change_world_timer.one_shot = true
	change_world_timer.connect("timeout", Callable.create(self,"change_world"))
	add_child(change_world_timer)
	drift_points.text = "Drift Points " + str(Global.reso.drift_points)
	repositionandresize(Global.reso.res)

func _process(_delta):
	visible = !Global.in_cutscene
	drift_points.text = "Drift Points : " + str(int(Global.reso.drift_points))
	if Global.show_obj:
		cur_objective.text = Global.cur_objective
	else:
		cur_objective.text = ""
	if Input.is_action_just_pressed("ui_cancel"):
		on_esc_pressed()
func revive():
	Global.in_cutscene = false
	Global.show_obj = true
	get_parent().health = 100
	get_parent().visible = true
	get_parent().health_bar_3d.change_health(100,true)
	
func repositionandresize(res:Vector2i):
	left.position = Vector2(res.y/1080.0*370.43, res.y/1080.0*10.0)
	left.scale = Vector2(res.y/1080.0*1.444, res.y/1080.0*1.393)
	right.position = Vector2(res.y/1080.0*435.0, res.y/1080.0*-232.0)
	right.scale = left.scale
	accelerate_button.position = Vector2(res.y/1080.0*-310.0, res.y/1080.0*-417.0)
	accelerate_button.scale = Vector2(res.y/1080.0*1.576, res.y/1080.0*1.358)
	brake.position = Vector2(res.y/1080.0*-708.0, res.y/1080.0*-223.0)
	brake.scale = accelerate_button.scale
	weapon_switch.position = Vector2(res.y/1080.0*55.0, res.y/1080.0*568.0)
	weapon_switch.scale = Vector2(res.y/1080.0*0.35, res.y/1080.0*0.35)
	print("Rezised")
	# esc.scale = Vector2(res.y/1080.0, res.y/1080.0)
	# cur_objective.offset_top = res.y/1080.0*100
	# drift_points.offset_top = res.y/1080.0*10
	# drift_points.offset_left = res.y/1080.0*-600

func add_drift_points(points:float):
	Global.reso.drift_points += points*kill

func on_esc_pressed():
	pausemenu.pause()
	pausemenu.show()
	hide()
	# plane.call("Pause")
	if Global.reso.volume != -15:
		var audio_bus = AudioServer.get_bus_index("Master")
		AudioServer.set_bus_mute(audio_bus, true)
	ResourceSaver.save(Global.reso,"user://Int/Res.tres")
