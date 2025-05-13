extends Node3D
var Main_menu : String = "res://Scenes/Main_menu.tscn"

func change_world():
	# Load the new world scene
	
	LoadingScreen.load_scene(Main_menu)
	
func _input(event) -> void:
	if event is InputEventScreenTouch and event.pressed:
		LoadingScreen.load_scene(Main_menu)
