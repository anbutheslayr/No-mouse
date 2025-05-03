@tool
extends Node3D
class_name Minigun
@onready var cylinder: MeshInstance3D = $Cylinder
@export var target_speed: int = 5000
@export var speed: int = 0
var cur_speed: int = 0
func _process(delta: float) -> void:
	if cylinder:
		speed = lerp(speed, target_speed, delta)
		cur_speed = lerp(cur_speed, speed, delta)
		cylinder.rotation_degrees.x -=  cur_speed* delta
		