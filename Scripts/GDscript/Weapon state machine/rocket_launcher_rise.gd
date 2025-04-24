extends Gun_state
class_name RocketLauncherRiseState
@export var machine_gun_state_path : NodePath
@onready var machine_gun_state : MachineGunState = get_node(machine_gun_state_path)


func state_enter():
	print("Rocket Launcher Rise State")
	machine_gun_state.anim.play("Rocket_rise")
	await machine_gun_state.anim.animation_finished
	state_changed.emit(self, "Rocket_launcher")

func state_exit():
	print("Exit Rocket Launcher Rise State")
	pass

func state_process(delta):
	pass

func state_update(delta):
	pass