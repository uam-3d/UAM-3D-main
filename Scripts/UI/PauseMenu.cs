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

		HideMenu();
	}

	private void ShowMenu() => GetParent<CanvasLayer>().Show();
	private void HideMenu() => GetParent<CanvasLayer>().Hide();
	private void ShowSubMenu(string id) => GetNode<CanvasLayer>(id).Show();
	private void HideSubMenu(string id) => GetNode<CanvasLayer>(id).Hide();

	public override void _UnhandledInput(InputEvent @event)
	{
		if (Input.IsActionJustPressed("esc"))
		{ 
			gameManager.Set("inPauseMenu", !gameManager.Get("inPauseMenu").AsBool());
			if (gameManager.Get("inPauseMenu").AsBool())
			{
				ShowMenu();
				Input.MouseMode = Input.MouseModeEnum.Visible;
			}
			else
			{
				HideMenu();
				Input.MouseMode = Input.MouseModeEnum.Captured;
			}
		}
	}

	private void OnContinueButtonPressed() // close Pause Menu
	{
		HideMenu();
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
		GetNode<Label>("confirmMenu/fgRect/titleLabel").Text =
			"Are you sure you want to return \n to the Main Menu?";
		HideMenu();
		ShowSubMenu("confirmMenu");
	}

	private void OnQuitButtonPressed()// close the game, will be asked to confirm once
	{
		GetNode<Label>("confirmMenu/fgRect/titleLabel").Text =
			"Are you sure you want to Quit?";
		_inQuit = true;
		HideMenu();
		ShowSubMenu("confirmMenu");
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
		HideSubMenu("confirmMenu");
		ShowMenu();
	}

	public override void _Process(double delta)
	{
		// GD.Print(Input.MouseMode);
	}
}
