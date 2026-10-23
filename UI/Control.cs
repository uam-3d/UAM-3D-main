using Godot;
using System;

public partial class Control : Godot.Control
{
	public Node GameManager;
	
	public override void _Ready()
	{
		GameManager = GetNode("/root/GameManager");
	}

	// public override void _Process(double delta)
	// {
	// 	if (Input.MouseMode == Input.MouseModeEnum.Captured)
	// 	{
	// 		this.Hide();
	// 	}
	// 	else
	// 	{
	// 		this.Show();
	// 	}
	// }

	public override void _UnhandledInput(InputEvent @event)
	{
		if (Input.MouseMode == Input.MouseModeEnum.Captured && Input.IsActionJustPressed("tab"))
		{
			this.Show();
			GameManager.Set("inPauseMenu", true);
			Input.MouseMode = Input.MouseModeEnum.Visible;
		}
	}

	public override void _Process(double delta)
	{
		GD.Print(Input.MouseMode);
		
	}
}
