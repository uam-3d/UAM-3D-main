using Godot;

namespace UAM3D.Scripts.Maps;

public partial class TilemapTest : Node3D
{
	public enum PlayerMode
	{
		Survivor,
		Hunter,
		Creative
	}

	public PlayerMode Mode = PlayerMode.Survivor;
	
	public string mapName = "tilemapTest";
	public static Vector3 DIMENSION = new Vector3(16, 64, 16);
	public Vector3 Spawn;
	
	public override void _Ready()
	{
		Spawn = GetNode<Chunk>("StaticBody3D").Spawn;
		GetNode<Player.Player>("Player").Position = Spawn;
	}
}
