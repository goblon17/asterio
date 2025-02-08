using Godot;
using System;

public partial class PauseScreen : Control
{
	[Export]
	private VBoxContainer container;
	[Export]
	private Button resumeButton;
	[Export]
	private Button mainMenuButton;

	public override void _Ready()
	{
		container.Visible = false;
		resumeButton.Pressed += TogglePause;
		mainMenuButton.Pressed += OnMainMenuPressed;
	}

    public override void _ExitTree()
    {
		GetTree().Paused = false;
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventKey key)
		{
            if (key.Pressed && key.Keycode == Key.Escape)
            {
				TogglePause();
            }
        }
    }

	private void TogglePause()
	{
		GetTree().Paused = !GetTree().Paused;
		GD.Print(GetTree().Paused);
		container.Visible = GetTree().Paused;
		Input.MouseMode = GetTree().Paused ? Input.MouseModeEnum.Visible : Input.MouseModeEnum.Captured;
    }

	private void OnMainMenuPressed()
	{
		GetTree().ChangeSceneToPacked(GameValues.Instance.MainMenu);
	}
}
