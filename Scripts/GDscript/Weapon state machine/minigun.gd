extends Gun_state
class_name MinigunState
@export var machine_gun_state_path : NodePath # The path to the machine gun state node
@onready var machine_gun_state : MachineGunState = get_node(machine_gun_state_path) # The machine gun state node
@export var minigun1_path : NodePath # The path to the minigun node
@onready var minigun1 : Minigun = get_node(minigun1_path) # The minigun node
@export var minigun2_path : NodePath # The path to the minigun node
@onready var minigun2 : Minigun = get_node(minigun2_path) # The minigun node
@export var minigun1_shoot_audio_path : NodePath # The path to the
@onready var minigun1_shoot_audio : AudioStreamPlayer3D = get_node(minigun1_shoot_audio_path) # The audio stream player for minigun1
@export var minigun2_shoot_audio_path : NodePath # The path to the
@onready var minigun2_shoot_audio : AudioStreamPlayer3D = get_node(minigun2_shoot_audio_path) # The audio stream player for minigun2

func state_enter():
	print("Minigun State")
	minigun1.target_speed = 4000
	minigun2.target_speed = 4000
	machine_gun_state.anim.play("Minigun_shoot")
	minigun1_shoot_audio.play()
	minigun2_shoot_audio.play()
	

func state_exit():
	print("Exit Minigun State")
	minigun1.target_speed = 0
	minigun2.target_speed = 0
	minigun1_shoot_audio.stop()
	minigun2_shoot_audio.stop()
	state_changed.emit(self, "Minigun_descend")
	
func state_process(delta):
	pass

func state_update(delta):
	pass
