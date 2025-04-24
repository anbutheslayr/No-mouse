extends Gun_state
class_name RocketLauncherState

@export var machine_gun_state_path : NodePath
@onready var machine_gun_state : MachineGunState = get_node(machine_gun_state_path)
@export var tot_ammo : int = 45
@export var tot_magazines : int = 3
@export var rocket_launcher_mesh_path : NodePath
@onready var rocket_launcher_mesh : MeshInstance3D = get_node(rocket_launcher_mesh_path)
@onready var cur_magazines : int = tot_magazines
@onready var cur_ammo : int = tot_ammo
@onready var rock_timer : Timer = Timer.new()
@export var time_between_shots : float = 2
var launch : bool = false
func _ready() -> void:
	add_child(rock_timer)
	rock_timer.one_shot = true
	rock_timer.connect("timeout",Callable(self,"on_timeout"))


	
func state_enter():
	print("Rocket Launcher State")
	rock_timer.start(time_between_shots)
	pass

func state_exit():
	pass

func state_process(delta):
	machine_gun_state.ammo_text.text = "            Ammo : " + str(cur_ammo) + "/" + str(tot_ammo) + "(" + str(cur_magazines) + ") \n       " + str(DisplayServer.screen_get_size()) + "\n       FPS : " + str(Engine.get_frames_per_second()) + "\n       Enemies Alive : " + str(get_tree().get_nodes_in_group("Enemy").size())
	if cur_ammo > 0 and rocket_launcher_mesh.get_child_count() > 0 and launch:
		print("launching")
		rocket_launcher_mesh.get_child(rocket_launcher_mesh.get_child_count() - 1).launch_rocket()
		rocket_launcher_mesh.get_child(rocket_launcher_mesh.get_child_count() - 1).reparent(get_tree().current_scene)
		launch = false
		rock_timer.start(time_between_shots)
		cur_ammo -= 1
	elif cur_ammo <= 0 and cur_magazines > 0:
		cur_ammo = tot_ammo
		cur_magazines -= 1
	elif cur_ammo <= 0 and cur_magazines <= 0:
		pass
	if rocket_launcher_mesh.get_child_count() == 0:
		state_changed.emit(self, "Rocket_launcher_descend")


func state_update(delta):   
	pass

func on_timeout():
	launch = true