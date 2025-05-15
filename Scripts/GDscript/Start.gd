extends Node


# Declare member variables here. Examples:
# var a: int = 2
# var b: String = "text"
@onready var anim = get_node("Control/AnimationPlayer")
@onready var start_font = preload("res://title.tres")
@onready var splash_font = preload("res://Interface/Splash font.tres")

# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	#start_font.size = (get_window().size.y/1080)*64
	#splash_font.size = (get_window().size.y/1080)*80
	# ParticleLoader.loaded
	anim.play("Fade in")
	await anim.animation_finished
	load_main_menu()

func load_main_menu():
	get_tree().change_scene_to_file("res://Scenes/Worlds/Cutscene.tscn")	
	# get_tree().change_scene_to_file("res://Scenes/Main_menu.tscn")
# Called every frame. 'delta' is the elapsed time since the previous frame.
#func _process(delta: float) -> void:
#	pass
