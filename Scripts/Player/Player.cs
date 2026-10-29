using System;
using Godot;
using UAM3D.Data;
using UAM3D.Types;

namespace UAM3D.Scripts.Player;

/// <summary>
/// The main Class for the player.
/// </summary>
public partial class Player : CharacterBody3D
{
	[Signal] public delegate void HealthChangedEventHandler(float health);
	[Signal] public delegate void UpdateCoordsEventHandler(Vector3 coords);
	[Signal] public delegate void PlayerDiedEventHandler();

	public float MoveSpeed { get; set; } = 5.0f;
	public float JumpVelocity { get; set; } = 4.5f;
	public float FlySpeed { get; set; } = 6.0f;
	public float Acceleration { get; set; } = 25.0f;
	public float Deceleration { get; set; } = 30.0f;
	
	public float MouseSensitivity { get; set; } = 0.0025f;
	public Node3D Camera { get; set; }
	
	public float MaxHealth { get; set; } = 100;
	public float CurrentHealth { get; private set; }
	public Inventory Inventory { get; private set; }

	private bool _isFlying;
	private bool _isDead;
	private Vector2 _mouseInput = Vector2.Zero;

	private static readonly float MinPitch = Mathf.DegToRad(-89.0f);
	private static readonly float MaxPitch = Mathf.DegToRad(89.0f);

	public override void _Ready()
	{
		CurrentHealth = MaxHealth;
		Inventory = new Inventory(Constants.INITIAL_INVENTORY_SIZE);
		Camera ??= GetNode<Node3D>("Camera3D");
	}

	[Temp("2026-10-28")]
	public void AddInvItem()
	{
		Inventory.AddItem(Items.Stick);
		Inventory.AddItem(Items.OakWood);
	}

	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("leftclick"))
		{
			var gameManager = GetNodeOrNull("/root/GameManager");
			bool isPaused = gameManager?.Get("inMenu").AsBool() ?? false;

			if (!isPaused)
				Input.MouseMode = Input.MouseModeEnum.Captured;
		}
		else if (@event.IsActionPressed("ui_cancel"))
		{
			Input.MouseMode = Input.MouseModeEnum.Visible;
		}
		// ALSO REMOVE WHEN REMOVING Player@44-49
		else if (@event.IsActionPressed("debug_button"))
		{
			AddInvItem();
		}
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (!CanMove()) return;
		
		if (Input.MouseMode == Input.MouseModeEnum.Captured && @event is InputEventMouseMotion motion)
		{
			_mouseInput += motion.Relative * MouseSensitivity;
		}
	}

	public override void _Process(double delta)
	{
		EmitSignal(SignalName.UpdateCoords, GlobalPosition);
	}

	public override void _PhysicsProcess(double delta)
	{
		if (!CanMove())
		{
			Vector3 deadVelocity = Velocity;

			// The player should still fall even if they cannot move
			if (!IsOnFloor() && _isDead)
			{
				deadVelocity += GetGravity() * (float)delta;
			}
			
			// Slide to a stop
			deadVelocity.X = Mathf.MoveToward(deadVelocity.X, 0, Deceleration * (float)delta);
			deadVelocity.Z = Mathf.MoveToward(deadVelocity.Z, 0, Deceleration * (float)delta);
			
			Velocity = deadVelocity;
			MoveAndSlide();
			return;
		}

		ApplyMouseLook();
		
		Vector3 velocity = Velocity;
		
		velocity = _isFlying
			? ProcessFlightMovement(velocity, (float)delta)
			: ProcessGroundMovement(velocity, (float)delta);
		
		Velocity = velocity;
		MoveAndSlide();
	}

	private void ApplyMouseLook()
	{
		if (_mouseInput == Vector2.Zero) return;

		RotateY(-_mouseInput.X);
		
		if (Camera != null)
		{
			Vector3 camRot = Camera.Rotation;
			camRot.X = Mathf.Clamp(camRot.X - _mouseInput.Y, MinPitch, MaxPitch);
			Camera.Rotation = camRot;
		}

		_mouseInput = Vector2.Zero;
	}

	private Vector3 ProcessGroundMovement(Vector3 velocity, float delta)
	{
		if (!IsOnFloor())
		{
			velocity += GetGravity() * delta;
		}

		if (Input.IsActionJustPressed("spacebar"))
		{
			if (IsOnFloor())
			{
				velocity.Y = JumpVelocity;
			}
			else if (Constants.DEV)
			{
				_isFlying = true;
				velocity.Y = 0f;
			}
		}

		Vector2 inputDir = Input.GetVector("a", "d", "w", "s");
		Vector3 direction = (Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();

		float targetSpeed = direction != Vector3.Zero
			? MoveSpeed
			: 0f;
		float rate = direction != Vector3.Zero
			? Acceleration
			: Deceleration;

		Vector3 targetVelocity = direction * targetSpeed;
		velocity.X = Mathf.MoveToward(velocity.X, targetVelocity.X, rate * delta);
		velocity.Z = Mathf.MoveToward(velocity.Z, targetVelocity.Z, delta * rate);

		return velocity;
	}

	private Vector3 ProcessFlightMovement(Vector3 velocity, float delta)
	{
		if (IsOnFloor() && velocity.Y < 0)
		{
			_isFlying = false;
			return velocity;
		}

		float verticalInput = 0f;
		if (Input.IsActionPressed("spacebar")) verticalInput += 1.0f;
		if (Input.IsActionPressed("shift")) verticalInput -= 1.0f;

		velocity.Y = Mathf.MoveToward(velocity.Y, verticalInput * FlySpeed, Acceleration * delta);

		Vector2 inputDir = Input.GetVector("a", "d", "w", "s");
		Vector3 direction = (Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();

		Vector3 targetVelocity = direction * FlySpeed;
		velocity.X = Mathf.MoveToward(velocity.X, targetVelocity.X, Acceleration * delta);
		velocity.Z = Mathf.MoveToward(velocity.Z, targetVelocity.Z, Acceleration * delta);

		return velocity;
	}

	public void ChangeHealth(float amount)
	{
		if (_isDead) return;

		CurrentHealth = Mathf.Clamp(CurrentHealth + amount, 0f, MaxHealth);
		EmitSignal(SignalName.HealthChanged, CurrentHealth);

		if (CurrentHealth <= 0.0f)
		{
			Die();
		}
	}

	private void Die()
	{
		_isDead = true;
		_isFlying = false; // this should never be needed, but just in case
		EmitSignal(SignalName.PlayerDied);
	}

	public bool CanMove()
	{
		if (_isDead) return false;
		var gameManager = GetNodeOrNull("/root/GameManager");
		bool inMenu = gameManager?.Get("inMenu").AsBool() ?? false;
		if (inMenu) return false; 
		return true;
	}
}
