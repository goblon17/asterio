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
        CallDeferred(nameof(OnReady));
	}

    private void OnReady()
    {
        GameMusic.Instance.SetMusic(GameMusic.Music.Radio, false);
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
