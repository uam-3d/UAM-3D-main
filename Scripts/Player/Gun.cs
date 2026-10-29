using Godot;
using System;

/// <summary>
/// The gun is supposed to shoot bullets, which will be simple meshInstances. On impact, the bullets should destroy parts of the terrain
/// </summary>

public partial class Gun : StaticBody3D
{

	private PackedScene _bulletScene;
	private Node3D _gunpoint;

	public override void _Ready()
	{
		_bulletScene = GD.Load<PackedScene>("res://Scenes/Player/bullet.tscn");
		_gunpoint = GetNode<Node3D>("Gunpoint");
	}
	
	public override void _Input(InputEvent @event)
	{
		if (Input.IsActionJustPressed("leftclick"))
		{
			GD.Print("clicked");
			Area3D _bullet = (Area3D)_bulletScene.Instantiate();
			_bullet.GlobalPosition = _gunpoint.GlobalPosition;
		}
		// Area3D _bullet = (Area3D)_bulletScene.Instantiate();
		// _bullet.GlobalPosition = _gunpoint.GlobalPosition;
	}
}
