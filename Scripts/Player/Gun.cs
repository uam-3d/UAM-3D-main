using Godot;

namespace UAM3D.Scripts.Player;

/// <summary>
/// The gun is supposed to shoot bullets, which will be simple meshInstances. On impact, the bullets should destroy parts of the terrain
/// </summary>
public partial class Gun : StaticBody3D
{

    private PackedScene _bulletScene;
    private Node3D _gunpoint;
    private Player _player;
    private Camera3D _playerCamera;

    public override void _Ready()
    {
        _playerCamera = GetNode<Camera3D>("/root/tilemapTest/Player/Camera3D");
        _bulletScene = GD.Load<PackedScene>("res://Scenes/Player/bullet.tscn");
        _gunpoint = GetNode<Node3D>("Gunpoint");
        _player = GetParent<Player>();
    }
	
    public override void _Input(InputEvent @event)
    {
        if (Input.IsActionJustPressed("leftclick"))
        {
            Bullet bullet = (Bullet)_bulletScene.Instantiate();
            GetTree().GetFirstNodeInGroup("Map").AddChild(bullet);
            bullet.GlobalPosition = _gunpoint.GlobalPosition;
            bullet.cameraBasis = _playerCamera.GlobalTransform.Basis;
        }
    }
}