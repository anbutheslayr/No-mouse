@tool
extends Node3D

class_name ObjectPool

var pool: Array = []
var tracer_scene: PackedScene = preload("res://Assets/Models/Guns/tracer.tscn")
@export var initial_size: int = 20 # Number of instances to create initially
func _ready():
	for i in range(initial_size):
		var instance = tracer_scene.instantiate()
		get_tree().root.add_child(instance) # Add the instance to the scene tree
		pool.append(instance)
		instance.visible = false # Hide the instance initially
		print("Instance added to pool: ", instance.name) # Debug print to check instance names

func get_instance() -> Node:
	var instance = pool.pop_back()
	instance.visible = true # Reset visibility when reusing
	return instance
	

func return_instance(instance: Node) -> void:
	instance.visible = false # Hide the instance before returning it to the pool
	pool.append(instance)
