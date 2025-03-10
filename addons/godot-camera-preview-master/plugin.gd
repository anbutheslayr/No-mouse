@tool
extends EditorPlugin 

const CamPreview = preload("./cam_preview.tscn")
const PreviewButton = preload("./preview_button.tscn")
var cam_preview_instance
var button_instance

var cam_selected: Camera3D
var pcam: Camera3D
var rt: RemoteTransform3D

var eds = get_editor_interface().get_selection()

func _enter_tree():
	connect("main_screen_changed", Callable(self, "main_screen_changed"))
	cam_preview_instance = CamPreview.instance()
	get_editor_interface().get_editor_main_screen().add_child(cam_preview_instance)
	cam_preview_instance.toggle_window(false)
	
	button_instance = PreviewButton.instance()
	add_control_to_container(EditorPlugin.CONTAINER_SPATIAL_EDITOR_MENU, button_instance)
#	button_instance.connect("toggled", self, "preview_pressed")
	button_instance.connect("preview_toggled", Callable(self, "preview_pressed"))
	button_instance.connect("preview_clear", Callable(self, "preview_free"))
	
	eds.connect("selection_changed", Callable(self, "selection_changed"))
	
func _exit_tree():
	disconnect("main_screen_changed", Callable(self, "main_screen_changed"))
	button_instance.disconnect("preview_clear", Callable(self, "preview_free"))
	button_instance.disconnect("preview_toggled", Callable(self, "preview_pressed"))
	preview_free()
	if cam_preview_instance:
		cam_preview_instance.queue_free()
	if button_instance:
		button_instance.queue_free()
		
func _process(_delta):
	if cam_selected and pcam:
		pcam.fov = cam_selected.fov
		pcam.projection = cam_selected.projection
		pcam.size = cam_selected.size
		
func find_a_camera(root) -> Camera3D:
	if root is Camera3D:
		return root
	match button_instance.search_mode:
		1:
			return root.find_child(button_instance.search_name, true, false) as Camera3D
		2:
			return get_cam_recursive(root)
	return null 
	
func get_cam_recursive(root):
	var cam: Camera3D
	for child in root.get_children():
		if child is Camera3D:
			return child
		cam = get_cam_recursive(child)
	return cam
		
func selection_changed():
	var selected = eds.get_selected_nodes()
	if not selected.is_empty():
		var cam = find_a_camera(selected[0])
		if cam:
			if cam_selected:
				cam_selected.disconnect("tree_exiting", Callable(self, "cam_deleted"))
			cam_selected = cam
			#remove old camera and remote transform
			preview_free()
			pcam = Camera3D.new()
			rt = RemoteTransform3D.new()
			cam_preview_instance.get_vp().add_child(pcam)
			cam_preview_instance.toggle_vp(true)
			cam.add_child(rt)
			cam.connect("tree_exiting", Callable(self, "cam_deleted"))
			rt.remote_path = pcam.get_path()
			rt.use_global_coordinates = true

func cam_deleted():
	preview_free()
	cam_preview_instance.toggle_vp(false)
	cam_selected.disconnect("tree_exiting", Callable(self, "cam_deleted"))

func preview_free():
	if pcam:
		pcam.queue_free()
	if rt:
		rt.queue_free()
	cam_preview_instance.toggle_vp(false)

func show_all():
	if cam_preview_instance:
		cam_preview_instance.show()
	if button_instance:
		button_instance.show()

func hide_all():
	if cam_preview_instance:
		cam_preview_instance.hide()
	if button_instance:
		button_instance.hide()
		
func main_screen_changed(screen):
	if screen == "3D":
		show_all()
	else:
		hide_all()

func preview_pressed(toggle):
	cam_preview_instance.toggle_window(toggle)
