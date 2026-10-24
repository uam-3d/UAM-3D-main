using Godot;

namespace UAM3D.UI;

public partial class PauseMenu : Godot.Control
{
	public Node GameManager; // Global Script
	
	private Button _continueButton;
	private Button _settingsButton;
	private Button _controlsButton;
	private Button _mainMenuButton;
	private Button _quitButton;

	private Button _confirmYes;
	private Button _confirmNo;

	private bool _inQuit = false;
	private bool _inMainMenu = false;
	
	public override void _Ready() 
	{
		GameManager = GetNode("/root/GameManager");
		
		_continueButton = GetNode<Button>("/root/terrainExampleMap/pauseMenu/Control/continue");
		_continueButton.Pressed += OnContinueButtonPressed;														// Equivalent to assigning a Signal in GDScript
		_settingsButton = GetNode<Button>("/root/terrainExampleMap/pauseMenu/Control/settings");
		_settingsButton.Pressed += OnSettingsButtonPressed;
		_controlsButton = GetNode<Button>("/root/terrainExampleMap/pauseMenu/Control/controls");
		_controlsButton.Pressed += OnControlsButtonPressed;
		_mainMenuButton = GetNode<Button>("/root/terrainExampleMap/pauseMenu/Control/mainMenu");
		_mainMenuButton.Pressed += OnMainMenuButtonPressed;
		_quitButton = GetNode<Button>("/root/terrainExampleMap/pauseMenu/Control/quit");
		_quitButton.Pressed += OnQuitButtonPressed;

		_confirmYes = GetNode<Button>("/root/terrainExampleMap/pauseMenu/Control/confirmMenu/fgRect/yes");
		_confirmYes.Pressed += onYesPressed;
		_confirmNo = GetNode<Button>("/root/terrainExampleMap/pauseMenu/Control/confirmMenu/fgRect/no");
		_confirmNo.Pressed += onNoPressed;

	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (Input.MouseMode == Input.MouseModeEnum.Captured && Input.IsActionJustPressed("tab"))				// Open Pause Menu
		{																										// Input.MouseMode is set to "MouseModeEnum.Captured" in dev_player.gd line 15-18
			((CanvasLayer)GetParent()).Show();
			GameManager.Set("inPauseMenu", true);
			Input.MouseMode = Input.MouseModeEnum.Visible;
		}
	}

	private void OnContinueButtonPressed()																		// close Pause Menu
	{
		((CanvasLayer)GetParent()).Hide();
		GameManager.Set("inPauseMenu", false);
		Input.MouseMode = Input.MouseModeEnum.Captured;
	}
	
	private void OnSettingsButtonPressed()
	{
		// for example for adjusting volume, graphics settings, etc
	}
	
	private void OnControlsButtonPressed()
	{
		// remap buttons, will be added later
	}
	
	private void OnMainMenuButtonPressed()																		// return to the main scene, will be asked to confirm once
	{
		_inMainMenu = true;
		((Label)GetNode("/root/terrainExampleMap/pauseMenu/Control/confirmMenu/fgRect/titleLabel")).Text = "Are you sure you want to return \n to the Main Menu?";
		((CanvasLayer)GetParent()).Hide();
		((CanvasLayer)GetNode("/root/terrainExampleMap/pauseMenu/Control/confirmMenu")).Show();
		// GetTree().ChangeSceneToFile("res://UI/main_menu.tscn");
		// GameManager.Set("inPauseMenu", false);
	}

	private void OnQuitButtonPressed()																			// close the game, will be asked to confirm once
	{
		((Label)GetNode("/root/terrainExampleMap/pauseMenu/Control/confirmMenu/fgRect/titleLabel")).Text =
			"Are you sure you want to Quit?";
		_inQuit = true;
		((CanvasLayer)GetParent()).Hide();
		((CanvasLayer)GetNode("/root/terrainExampleMap/pauseMenu/Control/confirmMenu")).Show();
		// GameManager.Set("inPauseMenu", false);
		// GetTree().Quit();
	}

	private void onYesPressed()
	{
		if (_inQuit)
		{
			GameManager.Set("inPauseMenu", false);
			_inQuit = false;
			GetTree().Quit();
		}

		if (_inMainMenu)
		{
			GameManager.Set("inPauseMenu", false);
			_inMainMenu = false;
			GetTree().ChangeSceneToFile("res://UI/main_menu.tscn");
		}
	}

	private void onNoPressed()
	{
		_inQuit = false;
		_inMainMenu = false;
		((CanvasLayer)GetNode("/root/terrainExampleMap/pauseMenu/Control/confirmMenu")).Hide();
		((CanvasLayer)GetParent()).Show();
	}
	
}
