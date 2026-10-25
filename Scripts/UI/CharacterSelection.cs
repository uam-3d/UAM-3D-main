using Godot;

namespace UAM3D.Scripts.UI;

public partial class CharacterSelection : Control
{
	private Button _creative;
	private Button _hunter;
	private Button _survival;
	private Button _old;

	private bool _unc = false;
	
	public override void _Ready()
	{
		_creative = GetNode<Button>("creativeMode");
		_creative.Pressed += OnAnyButtonPressed;
		_hunter = GetNode<Button>("hunterMode");
		_hunter.Pressed += OnAnyButtonPressed;
		_survival = GetNode<Button>("survivalMode");
		_survival.Pressed += OnAnyButtonPressed;
		_old  = GetNode<Button>("oldMode");
		_old.Pressed += OnOldButtonPressed;
	}

	private void OnAnyButtonPressed() // Will always show seed selection if any of the three buttons are pressed
	{
		_unc = false;
		((CanvasLayer)GetNode("/root/characterSelection/seedSelection")).Show();
	}

	private void OnOldButtonPressed() // This is only here so anyone can conveniently see our first few flopped terrain generation concepts.
									  // Will either be removed in the final game completely or added as a small Easter Egg.
	{
		_unc = true;
		((CanvasLayer)GetNode("/root/characterSelection/seedSelection")).Show();
	} 
	
}
