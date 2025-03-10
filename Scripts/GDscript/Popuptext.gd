extends Node3D
class_name Popuptext

@onready var label : Label3D = get_node("Label3D")
@onready var anim : AnimationPlayer = get_node("AnimationPlayer")
@onready var tween : Tween =  self.create_tween()
var tweenlength

func play_anim(damage : String,spread : int,height : int,pos : Vector3, an : int):
    global_position = pos
    label.text = damage;
    match an:
        0:
            tweenlength = anim.get_animation("Damagedone").length
            anim.play("Damagedone")
        1:
            tweenlength = anim.get_animation("Damagereceive").length
            anim.play("Damagereceive")
        2:
            tweenlength = anim.get_animation("Boom").length
            anim.play("Boom")
        3:
            tweenlength = anim.get_animation("Close miss +10").length
            anim.play("Close miss +10")
    var rand = RandomNumberGenerator.new()
    var end_pos = Vector3(rand.randi_range(-spread,spread),height,rand.randi_range(-spread, spread)) + global_position
    tween.interpolate_property(self, "global_position" ,global_position,end_pos , tweenlength , Tween.TRANS_LINEAR, Tween.EASE_IN_OUT)
    tween.start()
        