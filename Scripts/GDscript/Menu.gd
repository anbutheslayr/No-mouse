extends Control

@onready var level_select : Control = get_parent().get_node("Select level")

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
