extends Gun_state
class_name MinigunRiseState
@export var machine_gun_state_path : NodePath
@onready var machine_gun_state : MachineGunState = get_node(machine_gun_state_path)

func state_enter():
	print("Minigun Rise State")
	machine_gun_state.anim.play("Minigun_rise")
	await machine_gun_state.anim.animation_finished
	state_changed.emit(self, "Minigun")

func state_exit():
	print("Exit Minigun Rise State")
	pass
func state_process(delta):
	pass
func state_update(delta):
	pass
