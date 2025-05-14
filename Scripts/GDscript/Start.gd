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
<<<<<<< HEAD
	# ParticleLoader.loaded
=======
	await ParticleLoader.loaded
>>>>>>> d22ba05983e933ea4e4e4164f1b84b644234d300
	anim.play("Fade in")
	await anim.animation_finished
	load_main_menu()

func load_main_menu():
<<<<<<< HEAD
	get_tree().change_scene_to_file("res://Scenes/Worlds/Cutscene.tscn")	
	# get_tree().change_scene_to_file("res://Scenes/Main_menu.tscn")
=======
	get_tree().change_scene_to_file("res://Scenes/Main_menu.tscn")
	

>>>>>>> d22ba05983e933ea4e4e4164f1b84b644234d300
# Called every frame. 'delta' is the elapsed time since the previous frame.
#func _process(delta: float) -> void:
#	pass
