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
    [Export]
    private double timeToExplosion;
    [Export]
    private Vector2 rotationSpeedRange;
    [Export]
    private CannonParticles explosionParticles;
    [Export]
    private Node3D ship;
    [Export]
    private Node3D asteroid;
    [Export]
    private Node3D asteroidEndPos;

    private RandomNumberGenerator rng = new RandomNumberGenerator();

    private Vector3 rotationSpeed;

	public override void _Ready()
	{
        GameMusic.Instance.SetMusic(GameMusic.Music.Radio, false);
		Input.MouseMode = Input.MouseModeEnum.Visible;
        score.Text = score.Text.Replace("{num}", GameValues.Instance.score.ToString());
        mainMenuButton.Pressed += OnMainMenuPressed;
        quitButton.Pressed += OnQuitPressed;
        float x = rng.RandfRange(rotationSpeedRange.X, rotationSpeedRange.Y);
        float y = rng.RandfRange(rotationSpeedRange.X, rotationSpeedRange.Y);
        float z = rng.RandfRange(rotationSpeedRange.X, rotationSpeedRange.Y);
        rotationSpeed = new Vector3(x, y, z);

        Tween tween = asteroid.CreateTween().SetParallel();
        tween.TweenProperty(asteroid, Node3D.PropertyName.GlobalPosition.ToString(), asteroidEndPos.GlobalPosition, timeToExplosion);
        tween.TweenMethod(Callable.From((double time) =>
        {
            asteroid.RotationDegrees = rotationSpeed * (float)time;
        }), 0, timeToExplosion, timeToExplosion);
        tween.SetParallel(false);
        tween.TweenCallback(Callable.From(() =>
        {
            explosionParticles.Fire();
            asteroid.QueueFree();
            ship.QueueFree();
            GameMusic.Instance.SetMusic(GameMusic.Music.None, true);
        }));
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
