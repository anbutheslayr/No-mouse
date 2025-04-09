extends Node
class_name Gun_state



signal state_changed
# Called when the state is entered
func state_enter():
	pass

# Called when the state is exited
func state_exit():
	pass

# Called to update the state logic (e.g., AI or game logic)
func state_update(delta):
	pass

# Called to process the state (e.g., physics or frame-specific logic)
func state_process(delta):
	pass

	
