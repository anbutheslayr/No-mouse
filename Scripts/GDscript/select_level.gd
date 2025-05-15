extends Control

	

func on_world1():
	Global.reso.cur_world = 1
	ResourceSaver.save(Global.reso, "user://Int/Res.tres")
	# var lod = get_tree().get_root().get_node("LoadingScreen")
	# lod.call("load_scene", get_tree().get_root().get_node("Main_menu"), "res://Scenes/Worlds/World.tscn")
	LoadingScreen.load_scene("res://Scenes/Worlds/First cutscene.tscn")
	# get_tree().unload_current_scene()
func on_world2():
	Global.reso.cur_world = 2
	ResourceSaver.save(Global.reso, "user://Int/Res.tres")
	LoadingScreen.load_scene("res://Scenes/Worlds/World2.tscn")

func on_cutscene():
	LoadingScreen.load_scene("res://Scenes/Worlds/Cutscene.tscn")
