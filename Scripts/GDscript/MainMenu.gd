extends Node3D



@onready var settings : Control = get_node("Settings")
@onready var menu : Control = get_node("Menu")
@onready var click : AudioStreamPlayer = get_node("Click")
@onready var back : AudioStreamPlayer = get_node("Back")

var reso : Resolution

func _ready() -> void:
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
		reso = ResourceLoader.load("res://Interface/Res.tres")
		ResourceSaver.save(reso,"user://Int/Res.tres")
		reso = ResourceLoader.load("user://Int/Res.tres")
	else :
		reso = ResourceLoader.load("user://Int/Res.tres")
	
func _process(_delta: float) -> void:
	if Input.is_action_just_pressed("Save"):
		var image = get_viewport().get_texture().get_image()
		image.flip_y()
		image.save_png("D:/Godot export/SS/MainMenu.png")

func clicked() -> void:
	click.play()

func backed() -> void:
	back.play()
