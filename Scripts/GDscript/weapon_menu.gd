@tool
extends Control

# Exported variables for customization in the editor
@export var sprite_size : Vector2 = Vector2(128,128)  # Size of the weapon icons
@export var centre_sprite_size : Vector2 = Vector2(128,128)  # Size of the center icon
@export var highlight_color : Color  # Color for the highlighted option
@export var bg_color : Color  # Background color of the menu
@export var line_color : Color  # Color of the dividing lines and arcs
@export var lock_color : Color  # Color of the lock icon
@export var outer_radius : int = 256  # Outer radius of the wheel
@export var inner_radius : int = 64  # Inner radius of the wheel
@export var points_per_arc : int = 32  # Number of points used to draw arcs
@export var line_width : int = 4  # Width of the dividing lines
@export var options : Array[WheelOption]  # Array of options in the wheel
@export var selected_option : int = 0  # Index of the currently selected option
@export var weapon_state_machine_path : NodePath  # Path to the weapon state machine node
@onready var weapon_state_machine : WeaponStateMachine = get_node(weapon_state_machine_path)  # Reference to the weapon state machine

@onready var reso : Resolution = preload("res://Interface/Res.tres")
@export var lock_texture : Texture2D  # Texture for the lock icon

func _ready():
	resize(reso.res)

# Function to draw the weapon menu
func _draw():
	# Draw the background circle
	draw_circle(Vector2.ZERO, outer_radius, bg_color, true, -1, true)
	var locked
	# Loop through each option to draw its segment and icon
	for i in range(options.size()):
		if reso.weapons.get(options[i].gun_name) == 0 or !reso.weapons.has(options[i].gun_name):
			locked = true  # Mark the option as locked if it is not available
		else:
			locked = false # Mark the option as unlocked if it is available
		var start_rad = (TAU * i) / options.size() - (PI / 2)  # Adjusted start angle of the segment
		var end_rad = (TAU * (i + 1)) / options.size() - (PI / 2)  # Adjusted end angle of the segment
		var mid_rad = (start_rad + end_rad) / 2  # Midpoint angle of the segment
		var point = Vector2.from_angle(mid_rad)  # Position for the icon
		var points_inner = PackedVector2Array()  # Points for the inner arc
		var points_outer = PackedVector2Array()  # Points for the outer arc

		# Highlight the selected option
		if selected_option == i:
			
			# Generate points for the arcs
			for j in range(points_per_arc + 1):
				var angle = start_rad + j * (end_rad - start_rad) / points_per_arc
				points_inner.append(Vector2.from_angle(angle) * inner_radius)
				points_outer.append(Vector2.from_angle(angle) * outer_radius)
				
			points_outer.reverse()  # Reverse the outer points for proper polygon drawing
			
			# Draw the highlighted segment
			draw_polygon(points_inner + points_outer, PackedColorArray([highlight_color]))
			# draw_polygon(points_inner + points_outer, PackedColorArray([highlight_color]))
		if locked:
			for j in range(points_per_arc + 1):
				var angle = start_rad + j * (end_rad - start_rad) / points_per_arc
				points_inner.append(Vector2.from_angle(angle) * inner_radius)
				points_outer.append(Vector2.from_angle(angle) * outer_radius)
			points_outer.reverse()  # Reverse the outer points for proper polygon drawing
			# Draw the locked segment
			draw_polygon(points_inner + points_outer, PackedColorArray([lock_color]))
		
		# Draw the lock icon if the option is locked
		if locked:
			draw_texture_rect(lock_texture, Rect2(point * ((inner_radius + outer_radius) / 2.0) - (sprite_size / 2), sprite_size), false)
		else:
			# Draw the weapon icon for the current option
			draw_texture_rect(options[i].gun_icon, Rect2(point * ((inner_radius + outer_radius) / 2.0) - (sprite_size / 2), sprite_size), false)
	
	# Draw the inner and outer arcs
	draw_arc(Vector2.ZERO, inner_radius, 0, TAU, 100, line_color, line_width, true)
	draw_arc(Vector2.ZERO, outer_radius, 0, TAU, 100, line_color, line_width, true)
	
	# Draw dividing lines between options if there are multiple options
	if options.size() > 1:
		for i in range(options.size()):
			var angle = (i * (TAU / options.size())) - (PI / 2)  # Adjusted angle for the dividing line
			var point = Vector2.from_angle(angle)
			draw_line(point * inner_radius, point * outer_radius, line_color, line_width, true)
		
		# Draw the icon of the selected option at the center
		draw_texture_rect(options[selected_option].gun_icon, Rect2(centre_sprite_size / -2, centre_sprite_size), false)

# Function to update the menu on each frame
func _process(_delta):
	queue_redraw()  # Request a redraw of the menu

	# Get the mouse position relative to the control
	var mouse_pos = get_local_mouse_position()
	var mouse_radius = mouse_pos.length()  # Distance of the mouse from the center

	# Check if the mouse is within the wheel's bounds
	if mouse_radius > inner_radius and mouse_radius < outer_radius:
		var mouse_rad = fposmod(mouse_pos.angle() + (PI / 2), TAU)  # Angle of the mouse position
		var sel_temp = ceil((mouse_rad / TAU) * options.size()) - 1  # Determine the selected option
		if !(reso.weapons.get(options[sel_temp].gun_name) == 0 or !reso.weapons.has(options[sel_temp].gun_name)):
			selected_option = sel_temp
	


func resize(res: Vector2) -> void:
	# Resize the menu based on the provided resolution
	sprite_size = Vector2(res.y / 1080 * sprite_size.x, res.y / 1080 * sprite_size.y)  # Adjust sprite size based on resolution
	centre_sprite_size = Vector2(res.y / 1080 * centre_sprite_size.x, res.y / 1080 * centre_sprite_size.y)  # Adjust center sprite size based on resolution
	outer_radius = res.y / 1080.0 * outer_radius  # Adjust outer radius based on resolution
	inner_radius = res.y / 1080.0 * inner_radius  # Adjust inner radius based on resolution
	points_per_arc = res.y / 1080.0 * points_per_arc  # Adjust points per arc based on resolution
	line_width = res.y / 1080.0 * line_width  # Adjust line width based on resolution

func on_esc():
	reso.cur_gun = options[selected_option].gun_name  # Set the current gun in the resolution resource
	ResourceSaver.save(reso, "user://Int/Res.tres")
	weapon_state_machine.change_gun(reso.cur_gun)  # Change the gun in the weapon state machine
	get_parent().hide()