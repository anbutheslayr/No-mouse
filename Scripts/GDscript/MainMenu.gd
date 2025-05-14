extends Node3D



@onready var settings : SettingsMenu = get_node("Settings")
@onready var menu : Control = get_node("Menu")
@onready var click : AudioStreamPlayer = get_node("Click")
@onready var back : AudioStreamPlayer = get_node("Back")

<<<<<<< HEAD

func _ready() -> void:
	Engine.time_scale = 1
	# menu.resize(Global.reso.res)
	settings.resize(Global.reso.res)
	settings.reposition(Global.reso.res)
	settings.on_resolution_changed(Global.reso.res_int)

=======
var reso : Resolution

func _ready() -> void:
	Engine.time_scale = 1
	verify_res()
	# menu.resize(reso.res)
	settings.resize(reso.res)
	settings.reposition(reso.res)
	settings.on_resolution_changed(reso.res_int)

func verify_res() -> void:
	var dir : DirAccess = DirAccess.open("user://")
	if !dir.dir_exists("user://Int"):
		dir.make_dir("user://Int")
	if !dir.file_exists("user://Int/Res.tres"):
		print("File not found")
		reso = ResourceLoader.load("res://Interface/Res.tres")
		ResourceSaver.save(reso,"user://Int/Res.tres")
		reso = ResourceLoader.load("user://Int/Res.tres")
	else :
		print("File found")
		reso = ResourceLoader.load("user://Int/Res.tres")
	
>>>>>>> d22ba05983e933ea4e4e4164f1b84b644234d300
func _process(_delta: float) -> void:
	if Input.is_action_just_pressed("Save"):
		var image = get_viewport().get_texture().get_image()
		image.flip_y()
		image.save_png("D:/Godot export/SS/MainMenu.png")

func clicked() -> void:
	click.play()

func backed() -> void:
	back.play()
