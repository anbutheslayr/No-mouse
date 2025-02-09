extends Node


# Declare member variables here. Examples:
# var a = 2
# var b = "text"
# Called when the node enters the scene tree for the first time.
onready var lodscrn = preload("res://Interface/Loading screen.tscn")
 
func load_scene(cur_scene,next_scene):
	var lod_inst = lodscrn.instance()
	get_tree().root.call_deferred("add_child",lod_inst)

	var loader = ResourceLoader.load_interactive(next_scene)
	cur_scene.queue_free()


	yield(get_tree().create_timer(0.5),"timeout")
	var i = 0
	while true:
		var error = loader.poll()
		printt("Started poll")
		if error==OK:
			var label = lod_inst.get_node("ColorRect/Label") as Label
			label.text = "Loading  ...." + str(int(float(loader.get_stage())/loader.get_stage_count()*100)) + "%"
			printt(str(float(loader.get_stage())/loader.get_stage_count()))
		elif error ==ERR_FILE_EOF:
			var scene = loader.get_resource().instance()
			get_tree().root.call_deferred("add_child",scene)
			lod_inst.queue_free()
			return
		i = i+1
		if(i%(int(loader.get_stage_count()*0.04)+1) == 0):
			yield(get_tree(),"idle_frame")

	

# Called every frame. 'delta' is the elapsed time since the previous frame.
#func _process(delta):
#	pass
