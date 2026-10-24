using Godot;

namespace UAM3D.Scripts.UI.HUD;

public partial class HudManager : Scripts.UI.PauseMenu
{
	private Label healthDisplay;
	private Label coordsDisplay;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		healthDisplay = GetNode<Label>("Health/HealthValue");
		coordsDisplay = GetNode<Label>("Coordinates/CoordinatesValue");

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
	public string FormatHealth(float health) => Mathf.RoundToInt(health).ToString("D3");
	
	public void UpdateCoords(Vector3 coords) => coordsDisplay.Text = FormatCoords(coords);
	
	public string FormatCoords(Vector3 coords) => $"X: {coords.X:F2}\nY: {coords.Y:F2}\nZ: {coords.Z:F2}";
}
