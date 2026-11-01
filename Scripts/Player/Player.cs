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
	[Signal] public delegate void HotbarSlotChangedEventHandler(byte slot);
	[Signal] public delegate void DamageTakenEventHandler();

	public bool FlyAllowed { get; private set; } = Constants.DEV;
	public float MoveSpeed { get; set; } = 5.0f;
	public float JumpVelocity { get; set; } = 4.5f;
	public float FlySpeed { get; set; } = 6.0f;
	public float Acceleration { get; set; } = 25.0f;
	public float Deceleration { get; set; } = 30.0f;
	
	public float MouseSensitivity { get; set; } = 0.0025f;
	public Node3D Camera { get; set; }
	
	/// <summary>
	/// The Maximum possible Health of the Player.
	/// </summary>
	public float MaxHealth { get; set; } = 100f;
	
	/// <summary>
	/// The current health of the player
	/// </summary>
	public float CurrentHealth { get; private set; }
	public Inventory Inventory { get; private set; }
	public InventorySlot[] SpecialSlots { get; } =
	[
		// --- HAND SLOTS ---
		new(), // [0] HAND SLOT 1
		new(), // [1] HAND SLOT 2
		new(), // [2] HAND SLOT 3
		
		// --- ARMOR SLOTS
		new() {takes = [ItemClass.Armor]}, // [3] HEAD
		new() {takes = [ItemClass.Armor]}, // [4] BODY
		new() {takes = [ItemClass.Armor]}, // [5] LEGS
		new() {takes = [ItemClass.Armor]}, // [6] FEET
	];
	public byte EquippedSlot { get; private set; }
	public float XP { get; private set; }
	public float Level { get; private set; }

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
		Inventory.AddItem(Items.Stick, Items.Stick.maxStackSize - 1);
		Inventory.AddItem(Items.OakWood, Items.OakWood.maxStackSize - 1);
	}

	/// <summary>
	/// The action which the Debug button calls.
	/// </summary>
	public void DebugAction()
	{
		AddInvItem();
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
		else if (@event.IsActionPressed("debug_button"))
		{
			DebugAction();
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
			else if (FlyAllowed)
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

	/// <summary>
	/// Changes the Player's Health via <see cref="SetHealth"/>
    /// </summary>
    /// <param name="amount">The amount by which to change the Health</param>
	public void ChangeHealth(float amount) => SetHealth(CurrentHealth + amount);
    
    /// <summary>
	/// Sets the Player's Health, clamped between 0 and <see cref="MaxHealth"/>
	/// </summary>
	/// <param name="amount">The amount of Health to set</param>
	public void SetHealth(float amount)
	{
		if (_isDead) return;

		CurrentHealth = Mathf.Clamp(amount, 0f, MaxHealth);
		EmitSignal(SignalName.HealthChanged, CurrentHealth);

		if (CurrentHealth <= 0.0f) Die();
	}
	
	/// <summary>
	/// Makes the player take a certain amount of damage, after defense points taken into account.
	/// </summary>
	/// <param name="dmg">The amount of damage to apply.</param>
	public void TakeDamage(float dmg)
	{
		float def = 0;

		if (SpecialSlots[3].item is Armor head)
		{
			def += head.baseDefense;
		}

		if (SpecialSlots[4].item is Armor body)
		{
			def += body.baseDefense;
		}

		if (SpecialSlots[5].item is Armor legs)
		{
			def += legs.baseDefense;
		}

		if (SpecialSlots[6].item is Armor feet)
		{
			def += feet.baseDefense;
		}

		float healthToChange = Mathf.Max(dmg - def, 0); // stop the player from gaining health if the defense is too high
		EmitSignal(SignalName.DamageTaken); // USE THIS SIGNAL WHEN ADDING DAMAGE SOUNDS/VISUALS
		ChangeHealth(-healthToChange);
	}
	
	public void Attack()
	{
		float dmg;

		switch (SpecialSlots[EquippedSlot].item)
		{
			// Default to Punching
			case null:
				dmg = 1f;
				break;
			case Weapon weapon:
				dmg = weapon.baseAttack;
				break;
			default:
				throw new Exception("how did this happen"); // TODO: add better error message
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
		return !inMenu;
	}

	public void SwitchHotbarSlot(byte slot)
	{
		EquippedSlot = slot;
		EmitSignal(SignalName.HotbarSlotChanged, slot);
	}
}
