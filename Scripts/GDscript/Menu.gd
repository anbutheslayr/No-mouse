extends Control

@onready var level_select : Control = get_parent().get_node("Select level")
@onready var title_label : Label = $MarginContainer/VBoxContainer/Label
@onready var vb1 : VBoxContainer = $MarginContainer/VBoxContainer
@onready var vb2 : VBoxContainer = $MarginContainer/HBoxContainer/VBoxContainer
var title_theme : Theme = preload("res://Scenes/title_theme.tres")
func _ready():
	resize(Global.reso)
func on_play_pressed():
	get_parent().clicked()
	hide()
	level_select.show()

func on_settings_pressed():
	get_parent().clicked()
	hide()
	get_parent().settings.show()

func on_quit_pressed():
	get_parent().backed()
	get_tree().quit()

func on_insta_pressed():
	OS.shell_open("https://www.instagram.com/anbu_the_coder/profilecard/?igsh=MTBmbm83Z3hwcjI3bw%3D%3D")

func resize(resol : Resolution):
	title_label.add_theme_constant_override("shadow_offset_x", resol.res.y/1080.0*7)
	title_label.add_theme_constant_override("shadow_offset_y", resol.res.y/1080.0*7)
	title_label.add_theme_constant_override("shadow_outline_size", resol.res.y/1080.0*12)
	title_label.add_theme_constant_override("outline_size", resol.res.y/1080.0*7)
	# title_label.add_theme_constant_override("font_size", resol.res.y/1080.0*120)
	title_theme.default_font_size = resol.res.y/1080.0*120
	vb1.add_theme_constant_override("separation", resol.res.y/1080.0*(70))
	vb2.add_theme_constant_override("separation", resol.res.y/1080.0*(30))

	print("menu resized")
	print("title label size: ", title_label.get_theme_constant("font_size"))
	