extends Node
class_name WeaponStateMachine
# This is a state machine for the weapon system
# It handles the different states of the weapon and transitions between them
# It also handles the input and animation for the weapon
<<<<<<< HEAD
=======
@onready var reso : Resolution = ResourceLoader.load("user://Int/Res.tres")
>>>>>>> d22ba05983e933ea4e4e4164f1b84b644234d300
var enemies : Array 
var closest_enemy
@export var player_path : NodePath
@onready var player : Node3D = get_node(player_path)
@export var weapon_switch_path : NodePath
@onready var weapon_switch : TouchScreenButton = get_node(weapon_switch_path)
var locked : bool = false

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
	
	closest_enemy = get_closest_enemy()
	

	
	

func _physics_process(delta: float) -> void:
	# Called every physics frame, updates the current state
	if cur_state:
		cur_state.state_update(delta)
	if locked:
		weapon_switch.visible = false
	else:
		weapon_switch.visible = true

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

func on_detection(body : Node):
	if body.is_in_group("Enemy") or body.is_in_group("Runnable"):
		enemies.append(body as Node3D)
	
func on_exit(body : Node):
	if body.is_in_group("Enemy") or body.is_in_group("Runnable"):
		enemies.erase((body as Node3D))

func get_closest_enemy() -> Node3D:
	var closest : Node3D = null
	var distance = 9999
	for i in enemies:
		var dist = i.global_position.distance_to(player.global_position)
		if dist < distance or closest == null:
			closest = i
			distance = dist
	return closest

func change_gun(gun_name : String):
	if !gun_name == cur_state.name:
		on_state_changed(cur_state, gun_name + "_rise") # Change the state to the new gun name
