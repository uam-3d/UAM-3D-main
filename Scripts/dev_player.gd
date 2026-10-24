extends CharacterBody3D

var speed := 12.0
var a := 20.0

var cameraInputDirection := Vector2.ZERO
var mouseSensitivity := 0.25

@onready var cameraPivot: Node3D = $cameraPivot
@onready var camera3D: Camera3D = $cameraPivot/SpringArm3D/Camera3D

func _input(event: InputEvent) -> void:
	if GameManager.inPauseMenu:
		return
	if event.is_action_pressed("leftclick"):
		Input.mouse_mode = Input.MOUSE_MODE_CAPTURED
	if event.is_action_pressed("ui_cancel"):
		Input.mouse_mode = Input.MOUSE_MODE_VISIBLE

func _unhandled_input(event: InputEvent) -> void:
	var isCameraMotion := (
		event is InputEventMouseMotion and Input.get_mouse_mode() == Input.MOUSE_MODE_CAPTURED
	)
	if isCameraMotion:
		cameraInputDirection = event.screen_relative * mouseSensitivity

func _physics_process(delta: float) -> void:
	cameraPivot.rotation.x += cameraInputDirection.y * delta
	cameraPivot.rotation.x = clamp(cameraPivot.rotation.x, -PI/6, PI/3)
	cameraPivot.rotation.y -= cameraInputDirection.x * delta
	cameraInputDirection = Vector2.ZERO
	
	var rawInput := Input.get_vector("a", "d", "w", "s")
	var forward := camera3D.global_basis.z
	var right := camera3D.global_basis.x
	var moveDir := forward * rawInput.y + right * rawInput.x
	moveDir.y = 0.0
	moveDir = moveDir.normalized()
	velocity = velocity.move_toward(moveDir * speed, a * delta)
	
	var flyVector := Input.get_axis("shift", "spacebar")
	var targetVertical := flyVector * speed
	velocity.y = move_toward(velocity.y, targetVertical, a * speed)
	move_and_slide()
