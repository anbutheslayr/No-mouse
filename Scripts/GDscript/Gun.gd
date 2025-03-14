extends Node3D
class_name Gun

@export var raycastpath : NodePath
@export var animpath : NodePath
@export var gun_path : NodePath
@export var decal_path : String
@export var gun_damage : int = 5
@export var aim_speed : float = .25
@export var min_range : float = 2
@export var ammo : int
@export var start_magazines : int
@export var rocket_ammo : int = 15

@onready var raycast : RayCast3D = get_node(raycastpath)
@onready var anim : AnimationPlayer = get_parent().get_parent().get_node(animpath)
@onready var gun : MeshInstance3D= get_node(gun_path)
var decal : PackedScene = load(decal_path)
@onready var audiostreamplayer : AudioStreamPlayer = get_node("AudioStreamPlayer")
@onready var  marker : Node3D = get_node("Gun/Marker")
@onready var ammo_text : RichTextLabel = get_parent().get_parent().get_parent().get_node("Interface/Ammo_text")
var particle : PackedScene = preload("res://Scenes/Particles.tscn")
@onready var player : Node3D = get_parent().get_parent().get_parent()
@onready var rocket_launcher : Node3D = get_parent().get_node("Rocket launcher")
var roc_ammo : PackedScene = preload("res://Assets/Models/Guns/Rocket Ammo.tscn")
@onready var rocket_button : TextureButton = get_parent().get_parent().get_parent().get_node("Interface/Rocket_button")
@onready var popuptext : PackedScene = preload("res://Interface/Popup text.tscn")
var random
var cur_ammo
var cur_magazines
var rock_timer
var enemies : Array 
var closest_enemy
var cur_gun : int = 0
var entered = false
var launch = false



func _ready():  
    raycast.enabled = true
    random = RandomNumberGenerator.new()
    cur_ammo = ammo
    cur_magazines = start_magazines
    rock_timer = Timer.new()
    add_child(rock_timer)
    rock_timer.wait_time = .2
    rock_timer.one_shot = true
    rock_timer.connect("timeout",Callable(self,"on_timeout"))
    rock_timer.start()

func on_detection(body : Node):
    if body.is_in_group("Enemy") or body.is_in_group("Runnable"):
        enemies.append(body as Node3D)
    
func on_exit(body : Node):
    if body.is_in_group("Enemy") or body.is_in_group("Runnable"):
        enemies.erase((body as Node3D))

func _process(_delta: float) -> void:
    closest_enemy = get_closest_enemy()
    var direction = Vector3.ZERO
    ammo_text.text = "       Ammo = " + str(cur_ammo) + "/" + str(ammo) + "(" + str(cur_magazines) + ") \n       " + str(DisplayServer.screen_get_size()) + "\n       FPS : " + str(Engine.get_frames_per_second()) + "\n       Enemies Alive : " + str(get_tree().get_nodes_in_group("Enemy").size());
    
    if closest_enemy != null:
        direction = closest_enemy.global_position - global_position
    match cur_gun:
        0:
            if anim.current_animation == "Reload":
                ammo_text.text = "Reloading..."
            if enemies.size() > 0 and get_closest_enemy() != null and cur_ammo > 0 and anim.current_animation != "Reload":
                if entered == false and anim.current_animation != "Gun_descend":
                    anim.play("Gun_rise")
                    entered = true

                if gun.global_position.distance_to(closest_enemy.global_position) > min_range:
                    var aimspd = aim_speed
                    if get_closest_enemy().is_in_group("Runnable"):
                        aimspd = .95
                    gun.look_at(marker.global_position.lerp(global_position-direction,aimspd), Vector3.UP)
                    if anim.current_animation != "Shoot" and anim.current_animation != "Gun_rise" and anim.current_animation != "Gun_descend":
                        anim.play("Shoot")
            elif cur_ammo <= 0 and cur_magazines > 0:
                anim.play("Reload")
                cur_ammo = ammo
                cur_magazines -= 1
            elif entered == true and !anim.is_playing():
                anim.play("Gun_descend")
                entered = false


        1:

            ammo_text.text = "       Ammo = " + str(rocket_ammo*3) + "/" + str(45) + "(" + str(rocket_ammo) + ") \n       " + str(DisplayServer.screen_get_size()) + "\n       FPS : " + str(Engine.get_frames_per_second()) + "\n       Enemies Alive : " + str(get_tree().get_nodes_in_group("Enemy").size());
            if rocket_ammo > 0 and rocket_launcher.get_child_count() > 0:
                if entered == false and !anim.is_playing() and get_tree().get_nodes_in_group("Enemy").size() > 0:
                    anim.play("Rocket_rise")
                    entered = true
                if entered == true and !anim.is_playing() and launch:
                    rocket_launcher.get_child(rocket_launcher.get_child_count()-1).call("launch")
                    rocket_launcher.get_child(rocket_launcher.get_child_count()-1).reparent(get_tree().root.get_node("World"))
                    launch = false
                    rock_timer.start(2)
            elif enemies.size() > 0 and get_closest_enemy() != null and cur_ammo > 0 and rocket_launcher.get_child_count() == 0 :
                anim.play("Rocket_reload")
                entered = false
            elif entered == true and !anim.is_playing():
                anim.play_backwards("Rocket_rise")
                entered = false


func on_timeout():
    launch = true

func rocket_reload():
    var b1 = roc_ammo.Instance() as Node3D;
    rocket_launcher.AddChild(b1);
    b1.GlobalTransform = (rocket_launcher.GetParent().GetChild(0)as Node3D).GlobalTransform;
    var b2 = roc_ammo.Instance() as Node3D;
    rocket_launcher.AddChild(b2);
    b2.GlobalTransform = (rocket_launcher.GetParent().GetChild(1)as Node3D).GlobalTransform;
    var b3 = roc_ammo.Instance() as Node3D;
    rocket_launcher.AddChild(b3);
    b3.GlobalTransform = (rocket_launcher.GetParent().GetChild(2)as Node3D).GlobalTransform;
    rocket_ammo-=1
    gun_switch()

func gun_switch():
    if cur_gun == 0:
        cur_gun = 1
        rocket_button.Disabled = false
    else:
        cur_gun += 1
        rocket_button.Disabled = true
        if anim.is_playing() and anim.current_animation != "Gun_descend":
            anim.play("Gun_descend")
            entered = false

func on_shoot():
    
    if raycast.is_colliding() and cur_ammo > 0:
        var a = particle.instantiate() as Node3D;
        var b = decal.instantiate() as Node3D;
        get_tree().root.get_node("World").add_child(a)
        raycast.get_collider().add_child(b)
        a.global_position = raycast.get_collision_point()
        b.global_position = raycast.get_collision_point()
        if raycast.get_collision_normal() != Vector3.UP:
            b.look_at(raycast.get_collision_point() + raycast.get_collision_normal(), Vector3.UP)
        if raycast.get_collider().is_in_group("Enemy_Body"):
            var enemy = raycast.get_collider().get_parent().get_parent().get_parent() as Node3D
            enemy.call("Calculate_Health" , gun_damage)
            var d = popuptext.instantiate()
            get_tree().root.get_node("World").add_child(d)
            (d as Popuptext).play_anim( "Hit" , 10 , 5 , raycast.get_collision_point() + Vector3(0,1,0),1)
            
        if raycast.get_collider().is_in_group("Runnable"):
            raycast.get_collider().get_parent().get_parent().get_parent().call("Calculate_Health")
        audiostreamplayer.play()
    elif cur_ammo <= 0:
        cur_ammo = 1
    cur_ammo -= 1
 



func get_closest_enemy() -> Node3D:
    var closest : Node3D = null
    var distance = 9999
    for i in enemies:
        var dist = i.global_position.distance_to(player.global_position)
        if dist < distance or closest == null:
            closest = i
            distance = dist
    return closest


