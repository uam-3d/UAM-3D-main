using Godot;
using UAM3D.Scripts.UI.HUD;

namespace UAM3D.Scripts.Player;

/// <summary>
/// The Head Controller class for PlayerTest.
/// </summary>
public partial class PlayerTest : Node3D
{
	private HudManager hud;
	private Player player;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		hud = GetNode<HudManager>("HUD");
		player = GetNode<Player>("Players/Player");
		
		player.HealthChanged += (float health) => hud.UpdateHealth(health);
		player.UpdateCoords += (Vector3 coords) => hud.UpdateCoords(coords);
		// initialize the HUD
		hud.UpdateHealth(player.CurrentHealth);
	}
}
