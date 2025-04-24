extends Resource
class_name Resolution

@export var res : Vector2i = Vector2i(1280, 768)
@export var res_int : int = 0
@export var shadows : bool = true
@export var shadow_quality : int = 0
@export var glow : bool = true
@export var no_of_enemies : int = 6
@export var volume : int = 0
@export var drift_points : float = 0
@export var max_worlds : int = 2
@export var cur_world : int = 1
@export var difficulty : int = 0
@export var weapons : Dictionary[String, int]
@export var cur_gun : String = "Machine_gun"
