extends Node3D

func _ready():
	var style : DialogicStyle = load("res://dialogic/Style/style.tres")
	style.prepare()
func _process(_delta):
	if Input.is_action_just_pressed("ui_accept"):
		if Dialogic.current_timeline == null:
			# Dialogic.clear(DialogicGameHandler.ClearFlags.FULL_CLEAR)
			Dialogic.start('res://dialogic/Timelines/Intro.dtl')
		
