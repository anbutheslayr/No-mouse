extends Node

# Load the Resolution resource once and make it globally accessible
@onready var reso: Resolution
var in_cutscene: bool = true
var use_nav_for_player: bool = false
var player_nav_targ : Vector3
var show_obj: bool = false
var cur_objective : String = ""
var player_nav_speed :int = 0
var max_nav_angle :int = 10
var turn_lerp_speed :float = 10
var dialogic_action : InputEventAction = InputEventAction.new()

func _ready():
	# Ensure the resource exists, or create a new one
	var dir: DirAccess = DirAccess.open("user://")
	if !dir.dir_exists("user://Int"):
		dir.make_dir("user://Int")
	if !dir.file_exists("user://Res.tres"):
		print("Resolution resource not found. Creating a new one.")
		reso = preload("res://Interface/Res.tres").duplicate()
		ResourceSaver.save(reso, "user://Res.tres")
	else:
		reso = ResourceLoader.load("user://Int/Res.tres")
		print("Resolution resource loaded.")
	dialogic_action.action = "dialogic_default_action"
	dialogic_action.pressed = true

func _input(event: InputEvent) -> void:
	if event is InputEventScreenTouch:
		if event.pressed:
			# Input.action_press("dialogic_default_action")
			Input.parse_input_event(dialogic_action)
			print("Screen touched")
		# else:
		# 	Input.action_release("dialogic_default_action")
		# 	Input.parse_input_event(dialogic_action.pressed = false)

			# Handle enter key press here
