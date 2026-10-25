using Godot;
using UAM3D.Data;
using UAM3D.Types;

namespace UAM3D.Scripts.Player;

/// <summary>
/// The behavior script for the player.
/// </summary>
public partial class Player : CharacterBody3D
{
	private bool _flying;
	
	private const float Speed = 5.0f;
	private const float JumpVelocity = 4.5f;

	private const float MaxHealth = 100f;
	public float Health = MaxHealth;
	public Inventory Inventory = new(Constants.InitialInventorySize);

	private Node3D camera;
	private Vector2 cameraInputDirection = Vector2.Zero;
	private const float MouseSensitivity = 0.0025f;
	private const float MinCameraPitch = -Mathf.Pi / 2.0f;
	private const float MaxCameraPitch = Mathf.Pi / 2.0f;

	public override void _Ready()
	{
		camera = GetNode<Node3D>("Camera3D");
	}

	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("leftclick") && GetNode("/root/GameManager").Get("inPauseMenu").AsBool() == false)
		{
			Input.MouseMode = Input.MouseModeEnum.Captured;
		}
		else if (@event.IsActionPressed("ui_cancel"))
		{
			Input.MouseMode = Input.MouseModeEnum.Visible;
		}
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (@event is InputEventMouseMotion mouseMotion &&
			Input.MouseMode == Input.MouseModeEnum.Captured)
		{
			cameraInputDirection += mouseMotion.Relative * MouseSensitivity;
		}
	}

	public override void _Process(double delta)
	{
		EmitSignal(SignalName.UpdateCoords, GlobalPosition);
	}

	public override void _PhysicsProcess(double delta)
	{
		var playerRotation = Rotation;
		playerRotation.Y -= cameraInputDirection.X;
		Rotation = playerRotation;

		var cameraRotation = camera.Rotation;
		cameraRotation.X = Mathf.Clamp(
			cameraRotation.X - cameraInputDirection.Y,
			MinCameraPitch,
			MaxCameraPitch);
		camera.Rotation = cameraRotation;
		cameraInputDirection = Vector2.Zero;

		Vector3 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor() && !_flying)
		{
            velocity += GetGravity() * (float)delta;
		}
        

		// Handle Jump.
		if (Input.IsActionJustPressed("spacebar"))
		{
			if (IsOnFloor())
				velocity.Y = JumpVelocity;
			else if (Constants.Dev)
            {
                if (!_flying) 
                    _flying = true;
                else 
                    velocity.Y = JumpVelocity; 
            }
			
		}
        
        if (Input.IsActionJustPressed("shift") && _flying) velocity.Y = -JumpVelocity; 
        if (Input.IsActionJustReleased("shift") && _flying) velocity.Y = 0;
        if (Input.IsActionJustReleased("spacebar") && _flying) velocity.Y = 0;
		
		

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 inputDir = Input.GetVector("a", "d", "w", "s");
		Vector3 direction = (Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();
		if (direction != Vector3.Zero)
		{
			velocity.X = direction.X * Speed;
			velocity.Z = direction.Z * Speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
			velocity.Z = Mathf.MoveToward(Velocity.Z, 0, Speed);
		}


		Velocity = velocity;
		MoveAndSlide();
	}
	
	/// <summary>
	/// The signal for updated Health
	/// </summary>
	[Signal] public delegate void HealthChangedEventHandler(float health);
	[Signal] public delegate void UpdateCoordsEventHandler(Vector3 coords);
	
	/// <summary>
	/// Changes the Health of the player.
	/// </summary>
	/// <param name="dHealth">The change of health to affect the player.</param>
	public void ChangeHealth(float dHealth)
	{
		this.Health = Mathf.Clamp(this.Health + dHealth, 0, MaxHealth);
		EmitSignal(SignalName.HealthChanged, this.Health);
	}
}
