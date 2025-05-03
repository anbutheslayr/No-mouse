extends Gun_state
class_name MinigunDescendState
@export var machine_gun_state_path : NodePath
@onready var machine_gun_state : MachineGunState = get_node(machine_gun_state_path)

func state_enter():
	print("Minigun Descend State")
	machine_gun_state.anim.play_backwards("Minigun_rise")
	await machine_gun_state.anim.animation_finished
	state_changed.emit(self, "Idle")

func state_exit():
	print("Exit Minigun Descend State")
	pass
func state_process(delta):
	pass
func state_update(delta):
	pass
