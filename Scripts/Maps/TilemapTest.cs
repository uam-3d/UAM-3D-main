using Godot;
using UAM3D.Scripts.UI.HUD;
using UAM3D.Scripts.UI.Inventory;
using UAM3D.Scripts.Player;

namespace UAM3D.Scripts.Maps;

public partial class TilemapTest : Node3D
{
    private HudManager _hud;
    private Player.Player _player;
    private InventoryUI _inventoryUI;
	
	public string mapName = "tilemapTest";
	public static Vector3 DIMENSION = new Vector3(16, 64, 16);
	public Vector3 Spawn;
	
	public override void _Ready()
	{
		Spawn = GetNode<Chunk>("StaticBody3D").Spawn;
		GetNode<Player.Player>("Player").Position = Spawn;

        _hud = GetNode<HudManager>("HUD");
        _player = GetNode<Player.Player>("Player");
        _inventoryUI = GetNode<InventoryUI>("inventory/Control"); // remember "control"
        
        // --- PLAYER SIGNALS ---
        _player.HealthChanged += (float health) => _hud.UpdateHealth(health);
        _player.UpdateCoords += (Vector3 coords) => _hud.UpdateCoords(coords); 
        _hud.UpdateHealth(_player.CurrentHealth); // initialize the HUD
		
        // --- INVENTORY SIGNALS ---
        _inventoryUI.InventoryOpened += () => _inventoryUI.UpdateInventory(_player.Inventory);
        
    }
}