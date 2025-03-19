extends Node3D


@export var texture_progress_path : NodePath
@export var texture_progress_under_path : NodePath
@onready var texture_progress : TextureProgressBar = get_node(texture_progress_path)
@onready var texture_progress_under : TextureProgressBar = get_node(texture_progress_under_path)



func change_health(health : int):
    
    create_tween().tween_property(texture_progress, "value", health, 0.15).set_trans(Tween.TRANS_ELASTIC).set_ease(Tween.EASE_IN)
    create_tween().tween_property(texture_progress_under, "value", health, 0.7).set_trans(Tween.TRANS_CUBIC).set_ease(Tween.EASE_IN)

