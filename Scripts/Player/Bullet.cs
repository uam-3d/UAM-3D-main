using Godot;
using System;
using System.Threading;
using Timer = Godot.Timer;

/// <summary>
/// Bullet for Gun.cs
/// should damage the entity or destroy the terrain it collides with.
/// </summary>


public partial class Bullet : Area3D
{
	private Timer _timer;
	
	private static readonly Vector3 BulletDir = new Vector3(-1.0f, 0.0f, 0.0f);
	private static readonly float Speed = 50.0f;
	public Basis cameraBasis;
	
	public override void _Ready()
	{
		_timer = GetNode<Timer>("Timer");
		_timer.Timeout += OnTimerTimeout;
		BodyEntered += OnBodyEntered;
		_timer.WaitTime = 1.5f; // unit of WaitTime always in seconds
		_timer.Start();

	}
	public override void _PhysicsProcess(double delta)
	{
		// GlobalPosition += BulletDir * Speed * (float)delta;
		GlobalPosition += (-1) * cameraBasis.Z * (float)delta * Speed;
	}
	private void OnBodyEntered(Node3D body)
	{
		GD.Print("in Area");
	}

	private void OnTimerTimeout()
	{
		CallDeferred("queue_free");
	}
}
