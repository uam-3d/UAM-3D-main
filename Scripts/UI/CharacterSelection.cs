using Godot;

namespace UAM3D.Scripts.UI;

public partial class CharacterSelection : Control
{

	private Button _creative;
	private Button _hunter;
	private Button _survival;
	
	public override void _Ready()
	{
		_creative = GetNode<Button>("creativeMode");
		_creative.Pressed += OnAnyButtonPressed;
		_hunter = GetNode<Button>("hunterMode");
		_hunter.Pressed += OnAnyButtonPressed;
		_survival = GetNode<Button>("survivalMode");
		_survival.Pressed += OnAnyButtonPressed;
	}

	private void OnAnyButtonPressed() // Will always show seed selection if any of the three buttons are pressed
	{
		((CanvasLayer)GetNode("/root/characterSelection/seedSelection")).Show();
	}
	
}
