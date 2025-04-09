extends Gun_state
class_name MachineGunState
# This is the state for the machine gun weapon
# It handles the firing and reloading of the machine gun
# It also handles the input and animation for the machine gun

func state_enter():
    anim.play("Gun_rise")
    await anim.animation_finished

func state_exit():
    anim.play("Gun_descend")
    await anim.animation_finished
    
    

func state_process(delta):
    # Called to update the state logic (e.g., AI or game logic)
    if enemies.size() > 0 and get_closest_enemy() != null and cur_ammo > 0:
        if gun.global_position.distance_to(closest_enemy.global_position) > min_range:
            var aimspd = aim_speed
            if get_closest_enemy().is_in_group("Runnable"):
                aimspd = .95
            gun.look_at(marker.global_position.lerp(raycast.global_position - direction, aimspd), Vector3.UP)
            await anim.animation_finished
            anim.play("Shoot")
            on_shoot()
    elif cur_ammo <= 0 and cur_magazines > 0:
        anim.play("Reload")
        await anim.animation_finished
        cur_ammo = ammo
        cur_magazines -= 1
    else:
        state_changed.emit(self, "Idle")

func state_update(delta):
    pass
    
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

