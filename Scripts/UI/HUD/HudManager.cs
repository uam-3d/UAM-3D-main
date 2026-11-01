using Godot;

namespace UAM3D.Scripts.UI.HUD;

public partial class HudManager : Scripts.UI.PauseMenu
{
	private Label _healthDisplay;
	private Label _coordsDisplay;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_healthDisplay = GetNode<Label>("Health/HealthValue");
		_coordsDisplay = GetNode<Label>("Coordinates/CoordinatesValue");
	}

	/// <summary>
	/// Updates the Health Display on the HUD.
	/// </summary>
	/// <param name="health">The health of the player.</param>
	public void UpdateHealth(float health) => _healthDisplay.Text = FormatHealth(health);

	/// <summary>
	/// Formats the health of the player to an always-three-digit integer.
	/// </summary>
	/// <param name="health">The health of the player.</param>
	/// <returns></returns>
	private static string FormatHealth(float health) => Mathf.RoundToInt(health).ToString("D3");
	
	/// <summary>
	/// Updates the Coordinate Display on the HUD.
	/// </summary>
	/// <param name="coords">The Coordinates of the player.</param>
	public void UpdateCoords(Vector3 coords) => _coordsDisplay.Text = FormatCoords(coords);
	
	/// <summary>
	/// Formats the Coordinates of the player into three coordinates accurate to two decimal places.
	/// </summary>
	/// <param name="coords">The Coordinates of the player.</param>
	/// <returns></returns>
	private static string FormatCoords(Vector3 coords) => $"X: {coords.X:F2}\nY: {coords.Y:F2}\nZ: {coords.Z:F2}";
}
