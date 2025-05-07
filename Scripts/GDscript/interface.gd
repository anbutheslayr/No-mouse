extends Control

class_name interface

@onready var accelerate_button: TouchScreenButton = $Acceleration/Accelerate
@onready var brake : TouchScreenButton = $Acceleration/Brake
@onready var left : TouchScreenButton = $Steering/Left
@onready var right : TouchScreenButton = $Steering/Right
@onready var esc : TextureButton = $Esc
@onready var reso : Resolution = ResourceLoader.load("user://Int/Res.tres")
@onready var enemyspawner : Node3D = get_tree().get_nodes_in_group("Enemy_spawner")[0]
@onready var enemy_taxi_scene : PackedScene = ResourceLoader.load("Scenes/debug taxi.tscn")
@onready var timer : Timer = Timer.new()
@onready var enemy_spawntext : Label = $Enemy_spawntext 
@onready var pausemenu : Control = get_tree().get_nodes_in_group("Pause_menu")[0]
@onready var gun_switch : TouchScreenButton  = $Weapon_select
# @onready var plane : RigidBody3D = get_tree().get_nodes_in_group("Plane")[0]
@onready var drift_points : RichTextLabel = $Drift_points
@onready var change_world_timer : Timer = Timer.new()
# @onready var skeleton : PackedScene = ResourceLoader.load("res://Assets/Models/World2/Scenes/character-skeleton.tscn")
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
	drift_points.text = "Drift Points " + str(reso.drift_points)
	repositionandresize(reso.res)

func _process(_delta):
	drift_points.text = "Drift Points : " + str(int(reso.drift_points))
	enemy_spawntext.text = "Enemy " + str(cur_enemies) + "/" + str(reso.no_of_enemies) + " Spawning in " + str(int(timer.time_left))
	if cur_enemies == reso.no_of_enemies and get_tree().get_nodes_in_group("Enemy").size() != 0:
		enemy_spawntext.text = "All " + str(reso.no_of_enemies) + "/" + str(reso.no_of_enemies) + " enemies spawned"
		if reso.no_of_enemies >= min_kills and reso.cur_world < reso.max_worlds:
			change_world_timer.start(10)
	if cur_enemies == reso.no_of_enemies and get_tree().get_nodes_in_group("Enemy").size() == 0 and not dead and reso.no_of_enemies >= min_kills:
		if reso.cur_world < reso.max_worlds:
			enemy_spawntext.text = "\n\n Teleporting to next world in " + str(int(change_world_timer.time_left))
		else:
			enemy_spawntext.text = "You won against " + str(reso.no_of_enemies) + " enemies"
		won = true
	elif cur_enemies == reso.no_of_enemies and get_tree().get_nodes_in_group("Enemy").size() == 0 and not dead and reso.no_of_enemies < min_kills:
		enemy_spawntext.text = "\n\n\nAtleast defeat " + str(min_kills) + " enemies to get to next world \n You can change the number of enemies in settings"
		won = true
	if dead and not won:
		hide()
		get_parent().get_parent().get_node("Death_screen").show()
		get_parent().get_parent().get_node("Death_screen").call("Start")
		# plane.call("Pause")
	if Input.is_action_just_pressed("ui_cancel"):
		on_esc_pressed()

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
	# enemy_spawntext.offset_top = res.y/1080.0*100
	# drift_points.offset_top = res.y/1080.0*10
	# drift_points.offset_left = res.y/1080.0*-600

func add_drift_points(points:float):
	reso.drift_points += points*kill

func on_esc_pressed():
	pausemenu.pause()
	pausemenu.show()
	hide()
	# plane.call("Pause")
	if reso.volume != -15:
		var audio_bus = AudioServer.get_bus_index("Master")
		AudioServer.set_bus_mute(audio_bus, true)
	ResourceSaver.save(reso,"user://Int/Res.tres")

