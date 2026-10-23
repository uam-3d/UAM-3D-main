using System;
using Godot;

namespace UAM3D.UI.HUD;

public partial class hudManager : Control
{
	public Label healthDisplay;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		healthDisplay = GetNode<Label>("Health");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}

	public void FormatHealth()
	{
		throw new NotImplementedException();
	}
}