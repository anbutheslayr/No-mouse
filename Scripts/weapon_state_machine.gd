extends Node
class_name WeaponStateMachine
# This is a state machine for the weapon system
# It handles the different states of the weapon and transitions between them
# It also handles the input and animation for the weapon



@export var inital_state : Gun_state # The initial state of the weapon when the game starts
var cur_state : Gun_state # The current active state of the weapon
var states : Dictionary = {} # A dictionary to store all the states by their names

func _ready():
    # Called when the node is added to the scene
    # Initializes the states dictionary and sets the initial state
    for child in get_children():
        if child is Gun_state:
            states[child.name.to_lower()] = child # Add the state to the dictionary
            child.state_changed.connect(on_state_changed) # Connect the state_changed signal
    if inital_state:
        inital_state.state_enter() # Enter the initial state
        cur_state = inital_state # Set the current state to the initial state
    
        
    

            
func _process(delta: float) -> void:
    # Called every frame, handles the processing of the current state
    if cur_state:
        cur_state.state_process(delta)
    

    
    

func _physics_process(delta: float) -> void:
    # Called every physics frame, updates the current state
    if cur_state:
        cur_state.state_update(delta)

func on_state_changed(state, new_state_name):
    # Handles the transition between states when a state change is triggered
    if state != cur_state:
        return # Ignore if the state triggering the change is not the current state
    var new_state = states.get(new_state_name.to_lower()) # Get the new state by name

    if !new_state:
        return # Ignore if the new state does not exist
    if cur_state:
        cur_state.state_exit() # Exit the current state
    
    new_state.state_enter() # Enter the new state
    cur_state = new_state # Update the current state







