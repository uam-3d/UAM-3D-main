using Godot;

namespace UAM3D.Scripts;

public partial class DevPlayer : CharacterBody3D
{
	private bool flyAllowed;
	private float speed = 12f;
	private float a = 20f;
	
	private Vector2 cameraInputDirection = Vector2.Zero;
	private float mouseSensitivity = 0.25f;

	private Node3D cameraPivot;
	private Camera3D camera3D;
	
	public override void _Ready()
	{
		cameraPivot = GetNode<Node3D>("cameraPivot");
		camera3D = GetNode<Camera3D>("cameraPivot/SpringArm3D/Camera3D");
	}

	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("leftclick")) Input.MouseMode = Input.MouseModeEnum.Captured;
		if (@event.IsActionPressed("ui_accept")) Input.MouseMode = Input.MouseModeEnum.Visible;
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		bool isCameraMotion = @event is InputEventMouseMotion && Input.MouseMode == Input.MouseModeEnum.Captured;
		GD.Print(@event);
		
		if (isCameraMotion)
		{
			InputEventMouseMotion mouseMotionEvent = @event as InputEventMouseMotion;
			cameraInputDirection = mouseMotionEvent.Relative * mouseSensitivity;
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		var cameraPivotRotation = cameraPivot.Rotation;
		cameraPivotRotation.X += cameraInputDirection.Y * (float)delta;
		cameraPivotRotation.X = Mathf.Clamp(cameraPivotRotation.X, -Mathf.Pi / 6, Mathf.Pi / 3);
		cameraPivotRotation.Y += cameraInputDirection.X * (float)delta;
		cameraPivot.Rotation = cameraPivotRotation;
		cameraInputDirection = Vector2.Zero;

		var rawInput = Input.GetVector("a", "d", "w", "s");
		var forward = camera3D.GetGlobalBasis().Z;
		var right = camera3D.GetGlobalBasis().X;
		var moveDir = forward * rawInput.Y + right * rawInput.X;
		var flyVector = Input.GetAxis("shift", "spacebar");
		var targetVertical = flyVector * speed;
		moveDir.Y = flyAllowed
			? targetVertical
			: 0f;
		moveDir = moveDir.Normalized();
		Velocity = Velocity.MoveToward(moveDir * speed, a * (float)delta);
		MoveAndSlide();
	}
}