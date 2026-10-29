using Godot;
using UAM3D.Scripts.UI.HUD;
using UAM3D.Scripts.UI.Inventory;

namespace UAM3D.Scripts.Player;

/// <summary>
/// The Head Controller class for PlayerTest.
/// </summary>
public partial class PlayerTest : Node3D
{
	private HudManager _hud;
	private Player _player;
    private InventoryUI _inventoryUI;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_hud = GetNode<HudManager>("HUD");
		_player = GetNode<Player>("Players/Player");
        _inventoryUI = GetNode<InventoryUI>("inventory/Control"); // remember "control"
		
        // --- PLAYER SIGNALS ---
		_player.HealthChanged += (float health) => _hud.UpdateHealth(health);
		_player.UpdateCoords += (Vector3 coords) => _hud.UpdateCoords(coords);
		// initialize the HUD
		_hud.UpdateHealth(_player.CurrentHealth);
        
        // --- INVENTORY SIGNALS ---
        _inventoryUI.InventoryOpened += () => _inventoryUI.UpdateInventory(_player.Inventory);
        
    }
}
