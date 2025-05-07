extends Gun_state
class_name RocketLauncherDescendState
@export var machine_gun_state_path : NodePath
@onready var machine_gun_state : MachineGunState = get_node(machine_gun_state_path)

func state_enter():
	print("Rocket Launcher Descend State")
	machine_gun_state.anim.play_backwards("Rocket_rise")
	await machine_gun_state.anim.animation_finished
	state_changed.emit(self, "Idle")

func state_exit():
	print("Exit Rocket Launcher Descend State")
	(get_parent() as WeaponStateMachine).locked = false

func state_process(delta):
	pass

func state_update(delta):
	pass