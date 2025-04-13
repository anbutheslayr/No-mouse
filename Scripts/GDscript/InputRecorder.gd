extends Node

var recorded_inputs: Dictionary = {} # Stores recorded inputs with frame numbers as keys
var is_recording: bool = false
var is_replaying: bool = false
var replay_frame: int = 1 # Start frame count at 1
var recording_frame: int = 1 # Start recording frame count at 1

func start_recording():
	# Start recording inputs
	recorded_inputs.clear()
	is_recording = true
	recording_frame = 1 # Initialize recording frame to 1

func stop_recording():
	# Stop recording inputs
	is_recording = false
	# print("before save /n",recorded_inputs)
	save_recording("user://Int/recording.save")
	# print("after save /n",recorded_inputs)
func start_replay():
	# Start replaying recorded inputs
	load_recording("user://Int/recording.save")
	if recorded_inputs.size() > 0:
		is_replaying = true
		replay_frame = 1 # Initialize replay frame to 1

func stop_replay():
	# Stop replaying inputs
	print("Replay finished")
	is_replaying = false

func save_recording(file_path: String):
	# Save recorded inputs to a file
	
	
	var file = FileAccess.open(file_path, FileAccess.WRITE)
	
	file.store_var(recorded_inputs,true)
	file.close()
	print("Recording saved to %s" % file_path)
	

func load_recording(file_path: String):
	# Load recorded inputs from a file
	var file = FileAccess.open(file_path, FileAccess.READ)
	
	recorded_inputs = file.get_var(true)
	file.close()
	print("Recording loaded from %s" % file_path)

func _input(event):
	# Record inputs during recording mode
	if is_recording and event is InputEventKey:
		if not recorded_inputs.has(recording_frame):
			recorded_inputs[recording_frame] = []
		recorded_inputs[recording_frame].append(event.duplicate())

func _physics_process(delta: float) -> void:
	# Increment recording frame during recording
	if Input.is_action_just_pressed("Record"):
		print("Recording started")
		start_recording()
	if Input.is_action_just_pressed("Stop_recording"):
		print("Recording stopped")
		stop_recording()
	if Input.is_action_just_pressed("Start_replay"):
		print("Replay started")
		start_replay()
		
	if is_recording:
		recording_frame += 1

	# Replay inputs during replay mode
	if is_replaying:
		if recorded_inputs.has(replay_frame):
			for event in recorded_inputs[replay_frame]:
				Input.parse_input_event(event)
		replay_frame += 1
		if replay_frame > recorded_inputs.keys().max():
			stop_replay()
