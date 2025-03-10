@tool
extends Control

@onready var window: Panel = $window
@onready var vp: SubViewport = $window/container/vp
@onready var container: SubViewportContainer = $window/container


func _ready():
	resize_vp()

func get_vp() -> SubViewport:
	return vp
	
func toggle_window(toggle):
	window.visible = toggle
	
func toggle_vp(toggle):
	container.visible = toggle

func _on_window_resized():
	resize_vp()
	
func resize_vp():
	vp.size = window.size
	
