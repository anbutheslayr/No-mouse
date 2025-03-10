extends Control

@onready var vb1:VBoxContainer = get_node("MarginContainer/VBoxContainer")
@onready var vb2:VBoxContainer = get_node("MarginContainer/HBoxContainer/VBoxContainer")
@onready var inter_face:Control = get_parent().get_node("taxi/Interface")
@onready var plane:RigidBody3D = get_tree().get_nodes_in_group("Plane")[0] as RigidBody3D
@onready var revive:Button = get_node("MarginContainer/HBoxContainer/VBoxContainer/Revive")
var reso : Resolution = preload("user://Int/Res.tres")
var started = false
var revived = false


func _process(_delta: float) -> void:

    if started:
        Engine.time_scale = lerp(Engine.time_scale, 0, 0.03)
    if Engine.time_scale < 0.1:
        Engine.time_scale = 0
        if reso.volume != -15:
            var audio_bus = AudioServer.get_bus_index("Master")
            AudioServer.set_bus_mute(audio_bus, true)
        else :
            var audio_bus = AudioServer.get_bus_index("Master")
            AudioServer.set_bus_mute(audio_bus, false)

func start():
    started = true

func _on_restart_pressed() -> void:
    Engine.time_scale = 1
    if reso.volume != -15:
        var audio_bus = AudioServer.get_bus_index("Master")
        AudioServer.set_bus_mute(audio_bus, false)
    if reso.cur_world == 1:
        get_tree().change_scene_to_file("res://Scenes/Worlds/World.tscn")
    else:
        get_tree().change_scene_to_file("res://Scenes/Worlds/World2.tscn")


func _on_revive_pressed() -> void:
    return

func revive_debug():
    started = false
    hide()
    inter_face.revive()
    print("Revived")
    Engine.time_scale = 1

func ad_loaded():
    return

func on_rewarded():
    revive.text = "Resume"
    revive.disabled = false
    revived = true

func on_quittoMM_pressed():
    Engine.time_scale = 1
    get_tree().change_scene_to_file("res://Scenes/MainMenu.tscn")
    if reso.volume != -15:
        var audio_bus = AudioServer.get_bus_index("Master")
        AudioServer.set_bus_mute(audio_bus, false)
    reso.cur_world = 1
    ResourceSaver.save(reso,"user://Int/Res.tres")











