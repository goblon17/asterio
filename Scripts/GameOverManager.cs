using Godot;
using System;

public partial class GameOverManager : Node3D
{
	[Export]
	private RichTextLabel score;
	[Export]
	private Button mainMenuButton;
	[Export]
	private Button quitButton;

	public override void _Ready()
	{
		Input.MouseMode = Input.MouseModeEnum.Visible;
        score.Text = score.Text.Replace("{num}", GameValues.Instance.score.ToString());
        mainMenuButton.Pressed += OnMainMenuPressed;
        quitButton.Pressed += OnQuitPressed;
	}

    private void OnQuitPressed()
    {
        GetTree().Quit();
    }

    private void OnMainMenuPressed()
    {
        GetTree().ChangeSceneToPacked(GameValues.Instance.MainMenu);
    }
}
