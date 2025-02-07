using Godot;
using System;

public partial class MainMenuManager : Node3D
{
	[Export]
	private Button playButton;
	[Export]
	private Button quitButton;

	public override void _Ready()
	{
        playButton.Pressed += OnPlayPressed;
        quitButton.Pressed += OnQuitPressed;
	}

    private void OnPlayPressed()
    {
        GetTree().ChangeSceneToPacked(GameValues.Instance.Game);
    }

    private void OnQuitPressed()
    {
        GetTree().Quit();
    }
}
