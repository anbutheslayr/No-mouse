extends Node3D



@onready var settings : SettingsMenu = get_node("Settings")
@onready var menu : Control = get_node("Menu")
@onready var click : AudioStreamPlayer = get_node("Click")
@onready var back : AudioStreamPlayer = get_node("Back")


func _ready() -> void:
	Engine.time_scale = 1
	menu.resize(Global.reso)
	settings.resize(Global.reso.res)
	settings.reposition(Global.reso.res)
	settings.on_resolution_changed(Global.reso.res_int)

func _process(_delta: float) -> void:
	if Input.is_action_just_pressed("Save"):
		var image = get_viewport().get_texture().get_image()
		image.flip_y()
		image.save_png("D:/Godot export/SS/MainMenu.png")

func clicked() -> void:
	click.play()

func backed() -> void:
	back.play()
