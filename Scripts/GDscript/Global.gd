extends Node

# Load the Resolution resource once and make it globally accessible
@onready var reso: Resolution = preload("user://Int/Res.tres")
var in_cutscene: bool = true
var use_nav_for_player: bool = false
var player_nav_targ : Vector3
var show_obj: bool = false
var cur_objective : String = ""
func _ready():
	# Ensure the resource exists, or create a new one
	var dir: DirAccess = DirAccess.open("user://")
	if !dir.dir_exists("user://Int"):
		dir.make_dir("user://Int")
	if !dir.file_exists("user://Int/Res.tres"):
		print("Resolution resource not found. Creating a new one.")
		reso = preload("res://Interface/Res.tres").duplicate()
		ResourceSaver.save(reso, "user://Int/Res.tres")
	else:
		reso = ResourceLoader.load("user://Int/Res.tres")
		print("Resolution resource loaded.")