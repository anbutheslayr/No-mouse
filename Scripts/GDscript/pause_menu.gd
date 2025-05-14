extends Control

@onready var vb1: VBoxContainer = $MarginContainer/VBoxContainer
@onready var vb2: VBoxContainer = $MarginContainer/HBoxContainer/VBoxContainer
@onready var inter_face: Control = get_tree().get_nodes_in_group("Interface")[0] as Control
# @onready var plane: RigidBody3D = get_tree().get_nodes_in_group("Plane")[0] as RigidBody3D

<<<<<<< HEAD

func _ready() -> void:
	vb1.add_theme_constant_override("separation", int(Global.reso.res.y / 1080 * 70))
	vb2.add_theme_constant_override("separation", int(Global.reso.res.y / 1080 * 30))
=======
var reso: Resolution

func _ready() -> void:
	reso = load("user://Int/Res.tres")
	vb1.add_theme_constant_override("separation", int(reso.res.y / 1080 * 70))
	vb2.add_theme_constant_override("separation", int(reso.res.y / 1080 * 30))
>>>>>>> d22ba05983e933ea4e4e4164f1b84b644234d300

func on_resume_pressed() -> void:
	print("Resume")
	inter_face.show()
	get_tree().paused = false
	hide()
	# plane.call("Resume")
<<<<<<< HEAD
	if Global.reso.volume != -15:
=======
	if reso.volume != -15:
>>>>>>> d22ba05983e933ea4e4e4164f1b84b644234d300
		var audio_bus = AudioServer.get_bus_index("Master")
		AudioServer.set_bus_mute(audio_bus, false)
func pause():
	get_tree().paused = true

	
func on_quit_to_mm_pressed() -> void:
	get_tree().paused = false
<<<<<<< HEAD
	if Global.reso.volume != -15:
		var audio_bus = AudioServer.get_bus_index("Master")
		AudioServer.set_bus_mute(audio_bus, false)
	Global.reso.cur_world = 1
	ResourceSaver.save(Global.reso,"user://Int/Res.tres")
=======
	if reso.volume != -15:
		var audio_bus = AudioServer.get_bus_index("Master")
		AudioServer.set_bus_mute(audio_bus, false)
	reso.cur_world = 1
	ResourceSaver.save(reso,"user://Int/Res.tres")
>>>>>>> d22ba05983e933ea4e4e4164f1b84b644234d300
	LoadingScreen.load_scene("res://Scenes/Main_menu.tscn")
	
