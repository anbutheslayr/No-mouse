extends Node


# Declare member variables here. Examples:
# var a = 2
# var b = "text"
# Called when the node enters the scene tree for the first time.
@onready var lodscrn = preload("res://Interface/Loading screen.tscn")
var called = false
var lod_inst
var label : Label
var status = 0
var progress : Array = [float(0)]
var global_next_scene
var loaded = false
var new_scene : PackedScene
func load_scene(next_scene):
	called = true
	lod_inst = lodscrn.instantiate()
	get_tree().unload_current_scene()
	get_tree().root.call_deferred("add_child",lod_inst)
	ResourceLoader.load_threaded_request(next_scene)
	label = lod_inst.get_node("ColorRect/Label") as Label
	global_next_scene = next_scene

	
func _process(_delta: float) -> void:
	if called:
		status = ResourceLoader.load_threaded_get_status(global_next_scene,progress)
		print(progress[0])
		if status == ResourceLoader.THREAD_LOAD_LOADED:
			new_scene = ResourceLoader.load_threaded_get(global_next_scene)
			label.text = "Touch screen to continue"
			loaded = true
			set_process(false)
		else:
			label.text = "Loading  ...."	


func _input(event):
	if event is InputEventScreenTouch:
		if event.pressed and loaded and called:
			get_tree().change_scene_to_packed(new_scene)
			lod_inst.queue_free()
			loaded = false
			called = false
			new_scene = null
			status = null
			set_process(true)

# 	elif loaded and called:
# 		var scene = loader.get_resource().instantiate()
# 		get_tree().root.call("add_child",scene)
# 		lod_inst.queue_free()
# 		lod_inst = null
# 		loaded = false
# 		called = false
	

# Called every frame. 'delta' is the elapsed time since the previous frame.
#func _process(delta):
#	pass
