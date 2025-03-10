extends Node


# Declare member variables here. Examples:
# var a = 2
# var b = "text"
# Called when the node enters the scene tree for the first time.
@onready var lodscrn = preload("res://Interface/Loading screen.tscn")
var loaded = false
var called = false
var loader 
var lod_inst
func load_scene(cur_scene,next_scene):
	called = true
	lod_inst = lodscrn.instantiate()
	get_tree().root.call_deferred("add_child",lod_inst)
	cur_scene.queue_free()
	await cur_scene.tree_exited
	loader = ResourceLoader.load_threaded_request("res://Scenes/Worlds/null.tscn")
	loader.poll()
	loader = ResourceLoader.load_threaded_request(next_scene)
	


	await get_tree().create_timer(0.5).timeout
	var i = 0
	while true:
		var error = loader.poll()
		# printt("Started poll")
		if error==OK:
			var label = lod_inst.get_node("ColorRect/Label") as Label
			label.text = "Loading  ...." + str(int(float(loader.get_stage())/loader.get_stage_count()*100)) + "%"
			# printt(str(float(loader.get_stage())/loader.get_stage_count()))
		elif error ==ERR_FILE_EOF:
			loaded = true
			var label = lod_inst.get_node("ColorRect/Label") as Label
			label.text = "Tap screen to continue"
			# var scene = loader.get_resource().instance()
			# get_tree().root.add_child(scene)
			# lod_inst.queue_free()
			# lod_inst = null
			# loaded = false
			# called = false
			return
		i = i+1
		if(i%(int(loader.get_stage_count()*0.003)+1) == 0):
			await get_tree().idle_frame
	
		

func _input(event):
	if event is InputEventScreenTouch:
		if event.pressed and loaded and called:
			var scene = loader.get_resource().instantiate()
			get_tree().root.add_child(scene)
			lod_inst.queue_free()
			lod_inst = null
			loaded = false
			called = false
	elif loaded and called:
		var scene = loader.get_resource().instantiate()
		get_tree().root.call("add_child",scene)
		lod_inst.queue_free()
		lod_inst = null
		loaded = false
		called = false
	

# Called every frame. 'delta' is the elapsed time since the previous frame.
#func _process(delta):
#	pass
