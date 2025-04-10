using Godot;
using System;

public partial class GameManager : Node3D
{
	public static GameManager Instance { get; private set; }

    public override void _Ready()
    {
        Instance = this;
        GameValues.Instance.score = 0;
        GameMusic.Instance.SetMusic(GameMusic.Music.Normal, false);
        CallDeferred(nameof(OnReady));
    }

    private void OnReady()
    {
        Health.Instance.HealthChanged += OnHealthChanged;
    }

    private void OnHealthChanged(float current, float old)
    {
        if (current <= 0)
        {
            CallDeferred(nameof(GameOver));
        }
    }

    private void GameOver()
    {
        GetTree().ChangeSceneToPacked(GameValues.Instance.GameOver);
    }
}
