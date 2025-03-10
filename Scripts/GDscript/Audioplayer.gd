extends Node

@onready var att : AudioStreamPlayer = get_node("Att")

func _physics_process(_delta: float) -> void:
    
    if(get_tree().get_nodes_in_group("Enemy").size()> 0):
        att.playing = true
    else:
        att.playing = false

