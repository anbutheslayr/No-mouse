extends Gun_state
class_name IdleState
# This is the idle state for the weapon
# It handles the idle animation and input for the weapon

func state_enter():
    pass
func state_exit():
    pass
func state_update(delta):
    if enemies.size() > 0 and get_closest_enemy() != null and cur_ammo > 0:
        state_changed.emit(self, "Machine_gun")
func state_process(delta):
    pass
