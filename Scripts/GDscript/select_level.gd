extends Control

var reso : Resolution

func _ready():
    reso = ResourceLoader.load("user://Int/Res.tres")
    

func on_world1():
    reso.cur_world = 1
    ResourceSaver.save(reso, "user://Int/Res.tres")
    # var lod = get_tree().get_root().get_node("LoadingScreen")
    # lod.call("load_scene", get_tree().get_root().get_node("Main_menu"), "res://Scenes/Worlds/World.tscn")
    get_tree().change_scene_to_file("res://Scenes/Worlds/test.tscn")

func on_world2():
    reso.cur_world = 2
    ResourceSaver.save(reso, "user://Int/Res.tres")
    var lod = get_tree().get_root().get_node("LoadingScreen")
    lod.call("load_scene", get_tree().get_root().get_node("Main_menu"), "res://Scenes/Worlds/World2.tscn")

func on_cutscene():
    var lod = get_tree().get_root().get_node("LoadingScreen")
    lod.call("load_scene", get_tree().get_root().get_node("Main_menu"), "res://Scenes/Worlds/Cutscene.tscn")
