using Godot;
using System;
using System.Text.RegularExpressions;

/// <summary>
/// The goal of the PlayerCamera.cs code is to give the player more freedom with the camera.
/// The code currently includes a zooming function (key: c) and a second/third person view.
/// </summary>


public partial class PlayerCamera : Camera3D
{
	private Tween _tween;
	public enum ModeEnum
	{
		FIRST_PERSON, SECOND_PERSON, THIRD_PERSON
	}
	public ModeEnum cameraMode;

	public override void _Ready()
	{
		this.Position = new Vector3(0.0f, 0.5f, 0.0f);
		this.RotationDegrees = Vector3.Zero;
		cameraMode = ModeEnum.FIRST_PERSON;
	}
	
	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("c"))
		{
			if (_tween != null && _tween.IsValid())
			{
				_tween.Kill();
			}
			_tween = CreateTween();
			_tween.TweenProperty(this, "fov", 30f, 0.1f);
		}

		if (@event.IsActionReleased("c"))
		{
			_tween.Kill();
			this.Fov = 80.8f;
		}

		if (@event.IsActionPressed("f5"))
		{
			switch (cameraMode)
			{
				case ModeEnum.FIRST_PERSON:
					this.Position = new Vector3(0.0f, 2.0f, 5.0f);
					this.RotationDegrees = new Vector3(-25.0f, 0.0f, 0.0f);
					cameraMode = ModeEnum.SECOND_PERSON;
					break;
				case ModeEnum.SECOND_PERSON:
					this.Position = new Vector3(0.0f, 2.0f, -5.0f);
					this.RotationDegrees = new Vector3(-20.0f, 180.0f, 0.0f);
					cameraMode = ModeEnum.THIRD_PERSON;
					break;
				case ModeEnum.THIRD_PERSON:
					this.Position = new Vector3(0.0f, 0.5f, 0.0f);
					this.RotationDegrees = Vector3.Zero;
					cameraMode = ModeEnum.FIRST_PERSON;
					break;
			}
		}
	}
}
