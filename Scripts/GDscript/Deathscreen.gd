extends Control

@onready var vb1:VBoxContainer = get_node("MarginContainer/VBoxContainer")
@onready var vb2:VBoxContainer = get_node("MarginContainer/HBoxContainer/VBoxContainer")
@onready var inter_face:Control = get_parent().get_node("taxi/Interface")
@onready var revive:Button = get_node("MarginContainer/HBoxContainer/VBoxContainer/Revive")
var started = false
var revived = false

func _ready() -> void:
	vb1.add_theme_constant_override("separation", Global.reso.res.y/1080.0 * 70)
	vb2.add_theme_constant_override("separation", Global.reso.res.y/1080.0 * 20)
func _process(_delta: float) -> void:

	if started:
		inter_face.hide()
		await get_tree().create_timer(0.2).timeout
		if !get_tree().paused:
			Engine.time_scale = lerp(Engine.time_scale, 0.0, 0.13)
		print(Engine.time_scale)
		if Engine.time_scale < 0.1:
			get_tree().paused = true
			print("Paused")
			visible = true
	
		if Global.reso.volume != -15:
			var audio_bus = AudioServer.get_bus_index("Master")
			AudioServer.set_bus_mute(audio_bus, true)
		

func start():
	started = true
	Global.in_cutscene = true

func _on_restart_pressed() -> void:
	get_tree().paused = false
	Engine.time_scale = 1
	if Global.reso.volume != -15:
		var audio_bus = AudioServer.get_bus_index("Master")
		AudioServer.set_bus_mute(audio_bus, false)
	if Global.reso.cur_world == 1:
		LoadingScreen.load_scene("res://Scenes/Worlds/First cutscene.tscn")
	


func _on_revive_pressed() -> void:
	return

func revive_debug():
	get_tree().paused = false
	started = false
	Engine.time_scale = 1
	if Global.reso.volume != -15:
		var audio_bus = AudioServer.get_bus_index("Master")
		AudioServer.set_bus_mute(audio_bus, false)
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
	started = false
	Engine.time_scale = 1
	get_tree().paused = false
	LoadingScreen.load_scene("res://Scenes/Main_menu.tscn")
	Global.in_cutscene = true
	if Global.reso.volume != -15:
		var audio_bus = AudioServer.get_bus_index("Master")
		AudioServer.set_bus_mute(audio_bus, false)
	Global.reso.cur_world = 1
	ResourceSaver.save(Global.reso,"user://Int/Res.tres")
