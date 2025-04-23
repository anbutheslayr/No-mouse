@tool
extends Control

# Exported variables for customization in the editor
@export var sprite_size : Vector2 = Vector2(128,128)  # Size of the weapon icons
@export var highlight_color : Color  # Color for the highlighted option
@export var bg_color : Color  # Background color of the menu
@export var line_color : Color  # Color of the dividing lines and arcs
@export var outer_radius : int = 256  # Outer radius of the wheel
@export var inner_radius : int = 64  # Inner radius of the wheel
@export var points_per_arc : int = 32  # Number of points used to draw arcs
@export var line_width : int = 4  # Width of the dividing lines
@export var options : Array[WheelOption]  # Array of options in the wheel
@export var selected_option : int = 0  # Index of the currently selected option

# Function to draw the weapon menu
func _draw():
	# Draw the background circle
	draw_circle(Vector2.ZERO, outer_radius, bg_color, true, -1, true)
	
	# Loop through each option to draw its segment and icon
	for i in range(options.size()):
		var start_rad = (TAU * i) / options.size() - (PI / 2)  # Adjusted start angle of the segment
		var end_rad = (TAU * (i + 1)) / options.size() - (PI / 2)  # Adjusted end angle of the segment
		var mid_rad = (start_rad + end_rad) / 2  # Midpoint angle of the segment
		var point = Vector2.from_angle(mid_rad)  # Position for the icon

		# Highlight the selected option
		if selected_option == i:
			var points_inner = PackedVector2Array()  # Points for the inner arc
			var points_outer = PackedVector2Array()  # Points for the outer arc

			# Generate points for the arcs
			for j in range(points_per_arc + 1):
				var angle = start_rad + j * (end_rad - start_rad) / points_per_arc
				points_inner.append(Vector2.from_angle(angle) * inner_radius)
				points_outer.append(Vector2.from_angle(angle) * outer_radius)
				
			points_outer.reverse()  # Reverse the outer points for proper polygon drawing
			
			# Draw the highlighted segment
			draw_polygon(points_inner + points_outer, PackedColorArray([highlight_color]))

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
		draw_texture_rect(options[selected_option].gun_icon, Rect2(sprite_size / -2, sprite_size), false)

# Function to update the menu on each frame
func _process(_delta):
	queue_redraw()  # Request a redraw of the menu

	# Get the mouse position relative to the control
	var mouse_pos = get_local_mouse_position()
	var mouse_radius = mouse_pos.length()  # Distance of the mouse from the center

	# Check if the mouse is within the wheel's bounds
	if mouse_radius > inner_radius and mouse_radius < outer_radius:
		var mouse_rad = fposmod(mouse_pos.angle() + (PI / 2), TAU)  # Angle of the mouse position
		selected_option = ceil((mouse_rad / TAU) * options.size()) - 1  # Determine the selected option
		print(selected_option)  # Debug: Print the selected option index
