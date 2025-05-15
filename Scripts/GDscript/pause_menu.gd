extends Control

@onready var vb1: VBoxContainer = $MarginContainer/VBoxContainer
@onready var vb2: VBoxContainer = $MarginContainer/HBoxContainer/VBoxContainer
@onready var inter_face: Control = get_tree().get_nodes_in_group("Interface")[0] as Control
# @onready var plane: RigidBody3D = get_tree().get_nodes_in_group("Plane")[0] as RigidBody3D


func _ready() -> void:
	vb1.add_theme_constant_override("separation", int(Global.reso.res.y / 1080 * 70))
	vb2.add_theme_constant_override("separation", int(Global.reso.res.y / 1080 * 30))

func on_resume_pressed() -> void:
	print("Resume")
	inter_face.show()
	get_tree().paused = false
	hide()
	# plane.call("Resume")
	if Global.reso.volume != -15:
		var audio_bus = AudioServer.get_bus_index("Master")
		AudioServer.set_bus_mute(audio_bus, false)
func pause():
	get_tree().paused = true

	
func on_quit_to_mm_pressed() -> void:
	get_tree().paused = false
	if Global.reso.volume != -15:
		var audio_bus = AudioServer.get_bus_index("Master")
		AudioServer.set_bus_mute(audio_bus, false)
	Global.reso.cur_world = 1
	ResourceSaver.save(Global.reso,"user://Int/Res.tres")
	LoadingScreen.load_scene("res://Scenes/Main_menu.tscn")
	
