extends Control
class_name SettingsMenu


@onready var esc : TextureButton = get_node("TextureButton")
var title_font : FontFile = preload("res://Scenes/FONT.tres")
var gtheme : Theme = preload("res://Scenes/Theme.tres")
@onready var volume : HSlider =  get_node("MarginContainer/HBoxContainer2/VBoxContainer/Hslider")
@onready var resolution_button : OptionButton = get_node("MarginContainer/HBoxContainer2/VBoxContainer/OptionButton")
@onready var no_of_enemies : OptionButton = get_node("MarginContainer/HBoxContainer2/VBoxContainer/NoOfEnemies")
@onready var glow : CheckBox = get_node("MarginContainer/HBoxContainer2/VBoxContainer/Glow")
@onready var shadows : OptionButton = get_node("MarginContainer/HBoxContainer2/VBoxContainer/Shadow")
@onready var difficulty_selector : OptionButton = get_node("MarginContainer/HBoxContainer2/VBoxContainer/Difficulty")
var reso : Resolution = ResourceLoader.load("user://Int/Res.tres")

func _ready() -> void:
	if(reso.glow):
		glow.set_pressed_no_signal(true)
	

	resolution_button.add_item("1920x1080",0)
	resolution_button.add_item("1280x720",1)
	resolution_button.add_item("1024x576",2)
	resolution_button.add_item("800x480",3)
	resolution_button.add_item("640x360",4)
	resolution_button.selected = reso.res_int

	no_of_enemies.add_item("1",0)
	no_of_enemies.add_item("2",1)
	no_of_enemies.add_item("3",2)
	no_of_enemies.add_item("4",3)
	no_of_enemies.add_item("5",4)
	no_of_enemies.add_item("6",5)
	no_of_enemies.add_item("7",6)
	no_of_enemies.add_item("8",7)
	no_of_enemies.add_item("9",8)
	no_of_enemies.add_item("10",9)
	no_of_enemies.selected = reso.no_of_enemies-1

	shadows.add_item("Ultra low quality",0)
	shadows.add_item("Low quality",1)
	shadows.add_item("Medium quality",2)
	shadows.add_item("High quality",3)
	shadows.add_item("Ultra high quality",4)
	shadows.selected = reso.shadow_quality

	difficulty_selector.add_item("Easy",0)
	difficulty_selector.add_item("Normal",1)
	difficulty_selector.add_item("Hard",2)
	difficulty_selector.selected = reso.difficulty

	volume.set_value(reso.volume)


func on_difficulty_changed(index):
	match index:
		0:
			reso.difficulty = 0
		1:
			reso.difficulty = 1
		2:
			reso.difficulty = 2

func on_volume_changed(value):
	var audio_bus = AudioServer.get_bus_index("Master")
	AudioServer.set_bus_volume_db(audio_bus, value)
	if value < -14:
		AudioServer.set_bus_mute(audio_bus, true)
	else:    
		AudioServer.set_bus_mute(audio_bus, false)
	reso.volume = value

func on_glow_toggled(button_pressed):
	reso.glow = button_pressed
	if button_pressed:
		get_tree().root.get_node("Main_menu/WorldEnvironment").environment.glow_enabled = true
	else:
		get_tree().root.get_node("Main_menu/WorldEnvironment").environment.glow_enabled = false
func set_shadows(index):
	match index:
		0:
			ProjectSettings.set_setting("rendering/quality/directional_shadow/size", 1024)
			ProjectSettings.set_setting("rendering/quality/directional_shadow/size.mobile", 1024)
		1:
			ProjectSettings.set_setting("rendering/quality/directional_shadow/size", 2048)
			ProjectSettings.set_setting("rendering/quality/directional_shadow/size.mobile", 2048)
		2:
			ProjectSettings.set_setting("rendering/quality/directional_shadow/size", 4096)
			ProjectSettings.set_setting("rendering/quality/directional_shadow/size.mobile", 4096)
		3:
			ProjectSettings.set_setting("rendering/quality/directional_shadow/size", 4180)
			ProjectSettings.set_setting("rendering/quality/directional_shadow/size.mobile", 4180)
		4:
			ProjectSettings.set_setting("rendering/quality/directional_shadow/size", 4864)
			ProjectSettings.set_setting("rendering/quality/directional_shadow/size.mobile", 4864)
	reso.shadow_quality = index
func on_resolution_changed(index):
	match index:
		0:
			reso.res = Vector2i(1920,1080)
			reso.res_int = 0
		1:
			reso.res = Vector2i(1280,720)
			reso.res_int = 1
		2:
			reso.res = Vector2i(1024,576)
			reso.res_int = 2
		3:
			reso.res = Vector2i(800,480)
			reso.res_int = 3
		4:
			reso.res = Vector2i(640,360)
			reso.res_int = 4
	reso.res_int = index
	# get_window().content_scale_aspect = Window.CONTENT_SCALE_ASPECT_EXPAND
	# get_window().content_scale_mode = Window.CONTENT_SCALE_MODE_VIEWPORT
	get_window().content_scale_size = reso.res
	
	resize(reso.res)
	reposition(reso.res)

func resize(res):
	gtheme.default_font_size = (int)(res.x/1920*20)
	# gtheme.default_font.outline_size = (int)(res.x/1920*3)
	# title_font.size = (int)(res.x/1920*120)
	# title_font.outline_size = (int)(res.x/1920*3)
	# esc.size = Vector2(res.x/1920*96 , res.y/1080*96)
	pass

func reposition(res):
	var vb1 : VBoxContainer = get_node("MarginContainer/VBoxContainer")
	vb1.add_theme_constant_override("separation", (int)(res.y/1080*70))
	var vb2 = get_node("MarginContainer/HBoxContainer/VBoxContainer")
	vb2.add_theme_constant_override("separation", (int)(res.y/1080*20))
	var vb3 = get_node("MarginContainer/HBoxContainer2/VBoxContainer")
	vb3.add_theme_constant_override("separation", (int)(res.y/1080*20))
	var hb1 = get_node("MarginContainer/HBoxContainer")
	hb1.add_theme_constant_override("separation", (int)(res.x/1920*150))
	var hb2 = get_node("MarginContainer/HBoxContainer2")
	hb2.add_theme_constant_override("separation", (int)(res.x/1920*300))

func on_esc():
	ResourceSaver.save(reso, "user://Int/Res.tres")
	get_parent().backed()
	hide()
	get_parent().get_node("Menu").show()

func on_number_select(index:int):
	reso.no_of_enemies = index+1
