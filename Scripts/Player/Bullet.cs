using Godot;
using System;

/// <summary>
/// Bullet for Gun.cs
/// should damage the entity or destroy the terrain it collides with.
/// </summary>


public partial class Bullet : Area3D
{
	private static readonly Vector3 BulletDir = new Vector3(-1.0f, 0.0f, 0.0f);
    private static readonly float Speed = 50.0f; 
	public override void _Ready()
	{
		BodyEntered += OnBodyEntered;
	}

    public override void _PhysicsProcess(double delta)
    {
        GlobalPosition += BulletDir * Speed * (float)delta;
    }

    private void OnBodyEntered(Node3D body)
	{
		GD.Print("in Area");
	}
}
