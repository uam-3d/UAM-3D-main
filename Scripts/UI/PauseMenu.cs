using Godot;

namespace UAM3D.Scripts.UI;

/// <summary>
/// The PauseMenu Class.
///
/// @JIBO: 
/// Die Logik des Pause Menus ist sehr inelegant. Benutze Lieber Signals fuer die Yes and No Buttons
/// </summary>
public partial class PauseMenu : Control
{
	private Node gameManager; // Global Script
	
	private Button _continueButton;
	private Button _settingsButton;
	private Button _controlsButton;
	private Button _mainMenuButton;
	private Button _quitButton;

	private Button _confirmYes;
	private Button _confirmNo;

	private bool _inQuit;
	private bool _inMainMenu;

	private string map;
	
	public override void _Ready()
	{
		
		gameManager = GetNode("/root/GameManager");
		
		_continueButton = GetNode<Button>("continue");
		_continueButton.Pressed += OnContinueButtonPressed; // Equivalent to assigning a Signal in GDScript
		_settingsButton = GetNode<Button>("settings");
		_settingsButton.Pressed += OnSettingsButtonPressed;
		_controlsButton = GetNode<Button>("controls");
		_controlsButton.Pressed += OnControlsButtonPressed;
		_mainMenuButton = GetNode<Button>("mainMenu");
		_mainMenuButton.Pressed += OnMainMenuButtonPressed;
		_quitButton = GetNode<Button>("quit");
		_quitButton.Pressed += OnQuitButtonPressed;

		_confirmYes = GetNode<Button>("confirmMenu/fgRect/yes");
		_confirmYes.Pressed += OnYesPressed;
		_confirmNo = GetNode<Button>("confirmMenu/fgRect/no");
		_confirmNo.Pressed += OnNoPressed;

	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (Input.MouseMode == Input.MouseModeEnum.Captured && Input.IsActionJustPressed("tab")) // Open Pause Menu
		{ // Input.MouseMode is set to "MouseModeEnum.Captured" in dev_player.gd line 15-18
			((CanvasLayer)GetParent()).Show();
			gameManager.Set("inPauseMenu", true);
			Input.MouseMode = Input.MouseModeEnum.Visible;
		}
	}

	private void OnContinueButtonPressed() // close Pause Menu
	{
		((CanvasLayer)GetParent()).Hide();
		gameManager.Set("inPauseMenu", false);
		Input.MouseMode = Input.MouseModeEnum.Captured;
	}

	// @JIBO: Fuer dich ;)
	[Signal]
	public delegate void OnPauseEventHandler();
	[Signal]
	public delegate void OnResumeEventHandler();
	
	private void OnSettingsButtonPressed()
	{
		// for example for adjusting volume, graphics settings, etc
	}
	
	private void OnControlsButtonPressed()
	{
		// remap buttons, will be added later
	}
	
	private void OnMainMenuButtonPressed()// return to the main scene, will be asked to confirm once
	{
		_inMainMenu = true;
		((Label)GetNode("confirmMenu/fgRect/titleLabel")).Text =
			"Are you sure you want to return \n to the Main Menu?";
		((CanvasLayer)GetParent()).Hide();
		((CanvasLayer)GetNode("confirmMenu")).Show();
	}

	private void OnQuitButtonPressed()// close the game, will be asked to confirm once
	{
		((Label)GetNode("confirmMenu/fgRect/titleLabel")).Text =
			"Are you sure you want to Quit?";
		_inQuit = true;
		((CanvasLayer)GetParent()).Hide();
		((CanvasLayer)GetNode("confirmMenu")).Show();
	}

	private void OnYesPressed()
	{
		if (_inQuit)
		{
			gameManager.Set("inPauseMenu", false);
			_inQuit = false;
			GetTree().Quit();
		}

		if (_inMainMenu)
		{
			gameManager.Set("inPauseMenu", false);
			_inMainMenu = false;
			GetTree().ChangeSceneToFile("res://Scenes/UI/main_menu.tscn");
		}
	}

	private void OnNoPressed()
	{
		_inQuit = false;
		_inMainMenu = false;
		((CanvasLayer)GetNode("confirmMenu")).Hide();
		((CanvasLayer)GetParent()).Show();
	}

	public override void _Process(double delta)
	{
		// GD.Print(Input.MouseMode);
	}
}
