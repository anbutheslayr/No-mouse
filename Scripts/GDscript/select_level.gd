extends Control

<<<<<<< HEAD
	

func on_world1():
	Global.reso.cur_world = 1
	ResourceSaver.save(Global.reso, "user://Int/Res.tres")
	# var lod = get_tree().get_root().get_node("LoadingScreen")
	# lod.call("load_scene", get_tree().get_root().get_node("Main_menu"), "res://Scenes/Worlds/World.tscn")
	LoadingScreen.load_scene("res://Scenes/Worlds/test.tscn")
	# get_tree().unload_current_scene()
func on_world2():
	Global.reso.cur_world = 2
	ResourceSaver.save(Global.reso, "user://Int/Res.tres")
=======
var reso : Resolution

func _ready():
	reso = ResourceLoader.load("user://Int/Res.tres")
	

func on_world1():
	reso.cur_world = 1
	ResourceSaver.save(reso, "user://Int/Res.tres")
	# var lod = get_tree().get_root().get_node("LoadingScreen")
	# lod.call("load_scene", get_tree().get_root().get_node("Main_menu"), "res://Scenes/Worlds/World.tscn")
	LoadingScreen.load_scene("res://Scenes/Worlds/test.tscn")
func on_world2():
	reso.cur_world = 2
	ResourceSaver.save(reso, "user://Int/Res.tres")
>>>>>>> d22ba05983e933ea4e4e4164f1b84b644234d300
	LoadingScreen.load_scene("res://Scenes/Worlds/World2.tscn")

func on_cutscene():
	LoadingScreen.load_scene("res://Scenes/Worlds/Cutscene.tscn")
