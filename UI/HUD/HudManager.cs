using System;
using Godot;
using UAM3D.player;

namespace UAM3D.UI.HUD;

public partial class HudManager : PauseMenu
{
	public Label healthDisplay;
	public Node3D player;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		healthDisplay = GetNode<Label>("Health/HealthValue");
		
	}

	/// <summary>
	/// Updates the Health Display on the HUD.
	/// </summary>
	/// <param name="health">The health of the player.</param>
	public void UpdateHealth(float health) => healthDisplay.Text = FormatHealth(health);

	/// <summary>
	/// Formats the health of the player to an always-three-digit integer.
	/// </summary>
	/// <param name="health">The health of the player.</param>
	/// <returns></returns>
	public string FormatHealth(float health) => health.ToString("D3");
}
