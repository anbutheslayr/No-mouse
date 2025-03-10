extends Node3D

var frame = 0
var loaded = false
signal loaded
var p1 = preload("res://Assets/Materials/particle mat/Debris.tres")
var p2 = preload("res://Assets/Materials/particle mat/fire.tres")
var p3 = preload("res://Assets/Materials/particle mat/ghost smoke.tres")
var p4 = preload("res://Assets/Materials/particle mat/Smoke.tres")
var p5 = preload("res://Assets/Materials/particle mat/Particles.tres")
var p6 = preload("res://Assets/Materials/particle mat/Rocket particles.tres")
var materials = [p1,p2,p3,p4,p5,p6]
func _ready() -> void:
	for material in materials:
		var part_inst = Particles.new() as Particles
		part_inst.set_process_material(material)
		part_inst.one_shot = true
		part_inst.emitting = true
		self.add_child(part_inst)

func _physics_process(delta) -> void:
	if frame>=10:
		loaded = true
		set_physics_process(false)
		emit_signal("loaded")
	frame+=1


# Called every frame. 'delta' is the elapsed time since the previous frame.
#func _process(delta: float) -> void:
#	pass
