extends Gun_state
class_name MachineGunDescendState

@export var machine_gun_state_path : NodePath
@onready var machine_gun_state : MachineGunState = get_node(machine_gun_state_path)
func state_enter():
	print("Machine Gun Descend State")
	machine_gun_state.anim.play("Gun_descend")
	await machine_gun_state.anim.animation_finished
	state_changed.emit(self, "Idle")

func state_exit():
	print("Exit Machine Gun Descend State")

func state_update(delta):
	pass
	
func state_process(delta):
	pass