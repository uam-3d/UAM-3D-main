using Godot;
using UAM3D.player;
using UAM3D.UI.HUD;

namespace UAM3D.scenes;


/// <summary>
/// The Head Controller class for PlayerTest.
/// </summary>
public partial class PlayerTest : Node3D
{
	public HudManager HUD;
	public Player Player;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		HUD = GetNode<HudManager>("HUD");
		Player = GetNode<Player>("Players/Player");
		
		Player.HealthChanged += (float health) => HUD.UpdateHealth(health);
		// initialize the HUD
		HUD.UpdateHealth(Player.Health);
	}
}