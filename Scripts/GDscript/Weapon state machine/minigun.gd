extends Gun_state
class_name MinigunState
@export var machine_gun_state_path : NodePath # The path to the machine gun state node
@onready var machine_gun_state : MachineGunState = get_node(machine_gun_state_path) # The machine gun state node
@export var minigun1_path : NodePath # The path to the minigun node
@onready var minigun1 : Minigun = get_node(minigun1_path) # The minigun node
@export var minigun2_path : NodePath # The path to the minigun node
@onready var minigun2 : Minigun = get_node(minigun2_path) # The minigun node
@export var minigun1_shoot_audio_path : NodePath # The path to the
@onready var minigun1_shoot_audio : AudioStreamPlayer3D = get_node(minigun1_shoot_audio_path) # The audio stream player for minigun1
@export var minigun2_shoot_audio_path : NodePath # The path to the
@onready var minigun2_shoot_audio : AudioStreamPlayer3D = get_node(minigun2_shoot_audio_path) # The audio stream player for minigun2
@export var bullets : int = 100 # The number of bullets to shoot
@export var fire_rate : float = 0.05 # The time between shots
@onready var timer : Timer = Timer.new() # The timer for the fire rate
@onready var cur_bullets : int = bullets # The current number of bullets
@export var gun_damage : int = 5 # The damage of the gun
@export var machine_gun1_descend_audio_path : NodePath # The path to the audio stream player for the machine gun
@onready var machine_gun1_descend_audio : AudioStreamPlayer3D = get_node(machine_gun1_descend_audio_path) # The audio stream player for the machine gun
@export var machine_gun2_descend_audio_path : NodePath # The path to the audio stream player for the machine gun
@onready var machine_gun2_descend_audio : AudioStreamPlayer3D = get_node(machine_gun2_descend_audio_path) # The audio stream player for the machine gun



func state_enter():
	print("Minigun State")
	# minigun1.target_speed = 4000
	# minigun2.target_speed = 4000
	# machine_gun_state.anim.play("Minigun_shoot")
	minigun1.particle.emitting = true
	minigun2.particle.emitting = true
	minigun1_shoot_audio.play()
	minigun2_shoot_audio.play()
	add_child(timer)
	timer.wait_time = fire_rate
	timer.one_shot = true
	timer.start()
	timer.connect("timeout", Callable(self, "shoot"))
	

func state_exit():
	print("Exit Minigun State")
	minigun1.target_speed = 0
	minigun2.target_speed = 0
	minigun1.particle.emitting = false
	minigun2.particle.emitting = false

func shoot():
	if minigun1.raycast.is_colliding() and cur_bullets > 0:
		machine_gun_state.particles(minigun1.raycast)
		machine_gun_state.apply_damage(minigun1.raycast, gun_damage)
	if minigun2.raycast.is_colliding() and cur_bullets > 0:
		machine_gun_state.particles(minigun2.raycast)
		machine_gun_state.apply_damage(minigun2.raycast, gun_damage)
	elif cur_bullets <= 0:
		print("Out of ammo")
		minigun1.target_speed = 0
		minigun2.target_speed = 0
		minigun1_shoot_audio.stop()
		minigun2_shoot_audio.stop()
		state_changed.emit(self, "Minigun_descend")
	print("Bullets left: ", cur_bullets)
	
	

func state_process(delta):
	if timer.is_stopped():
		timer.start()
		shoot()
		cur_bullets -= 2

func state_update(delta):
	pass
