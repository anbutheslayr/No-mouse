extends Gun_state
class_name IdleState
# This is the idle state for the weapon
# It handles the idle animation and input for the weapon
@onready var weapon_state_machine : WeaponStateMachine = get_parent()
func state_enter():
	print("Idle State")
func state_exit():
	print("Exit Idle State")
func state_update(delta):
	pass
func state_process(delta):
	if weapon_state_machine.enemies.size() > 0:
		state_changed.emit(self, "Machine_gun")
		print("Switching to Machine Gun State")
