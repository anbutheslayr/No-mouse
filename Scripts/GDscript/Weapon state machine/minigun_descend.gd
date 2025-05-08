extends Gun_state
class_name MinigunDescendState
@export var machine_gun_state_path : NodePath
@onready var machine_gun_state : MachineGunState = get_node(machine_gun_state_path)
@export var minigun_state_path : NodePath
@onready var minigun_state : MinigunState = get_node(minigun_state_path)

func state_enter():
	print("Minigun Descend State")
	machine_gun_state.anim.play_backwards("Minigun_rise_without_audio")
	await machine_gun_state.anim.animation_finished
	state_changed.emit(self, "Idle")

func state_exit():
	print("Exit Minigun Descend State")
	(get_parent() as WeaponStateMachine).locked = false
func state_process(delta):
	if !minigun_state.minigun1.particle.emitting and !minigun_state.machine_gun1_descend_audio.is_playing():
		minigun_state.minigun1_shoot_audio.stop()
		minigun_state.minigun2_shoot_audio.stop()
		minigun_state.machine_gun1_descend_audio.play()
		minigun_state.machine_gun2_descend_audio.play()
		print("minigun descend audio")
	print(minigun_state.minigun1.cur_speed <1500 and !minigun_state.machine_gun1_descend_audio.is_playing())
func state_update(delta):
	pass
