extends Node3D

func _ready():
	Dialogic.clear()
	Dialogic.start_timeline("Intro")
