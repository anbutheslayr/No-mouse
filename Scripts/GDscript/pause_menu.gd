extends Control

@onready var vb1: VBoxContainer = $MarginContainer/VBoxContainer
@onready var vb2: VBoxContainer = $MarginContainer/HBoxContainer/VBoxContainer
@onready var inter_face: Control = get_tree().get_nodes_in_group("Interface")[0] as Control
# @onready var plane: RigidBody3D = get_tree().get_nodes_in_group("Plane")[0] as RigidBody3D

var reso: Resolution
var paused: bool = false

func _ready() -> void:
	reso = load("user://Int/Res.tres")
	vb1.add_theme_constant_override("separation", int(reso.res.y / 1080 * 70))
	vb2.add_theme_constant_override("separation", int(reso.res.y / 1080 * 30))
	paused = false

func on_resume_pressed() -> void:
	print("Resume")
	inter_face.show()
	Engine.time_scale = 1
	hide()
	# plane.call("Resume")
	if reso.volume != -15:
		var audio_bus = AudioServer.get_bus_index("Master")
		AudioServer.set_bus_mute(audio_bus, false)
	paused = false
func pause():
	paused = true
func _process(delta):
	if paused:
		Engine.time_scale = lerp(Engine.time_scale, 0.0001, 30 * delta)
	
func on_quit_to_mm_pressed() -> void:
	paused = false
	LoadingScreen.load_scene("res://Scenes/Main_menu.tscn")
	if reso.volume != -15:
		var audio_bus = AudioServer.get_bus_index("Master")
		AudioServer.set_bus_mute(audio_bus, false)
	reso.cur_world = 1
	ResourceSaver.save(reso,"user://Int/Res.tres")
