using Godot;
using System;

public partial class AsteroidSpawner : Node3D
{
    public static AsteroidSpawner Instance { get; private set; }

	[Export]
	private double cooldown;
    [Export]
    private float speed;
    [Export]
    private Vector2 rotationSpeedRange;
    [Export]
    private Vector2 halfExtents;
	[Export]
	private PackedScene asteroidPrefab;
    [Export]
    private AsteroidShape[] possibleShapes;

    public event Action<AsteroidBase> AsteroidDied;

    private RandomNumberGenerator rng = new RandomNumberGenerator();

    private double timer = double.MaxValue;

    public override void _EnterTree()
    {
        Instance = this;
    }

    public override void _ExitTree()
    {
        Instance = null;
    }

    public override void _Process(double delta)
    {
        if ((timer += delta) < cooldown)
        {
            return;
        }
        timer = 0;

        AsteroidBase asteroid = asteroidPrefab.Instantiate<AsteroidBase>();
        AddChild(asteroid, forceReadableName: true);
        asteroid.Position = new Vector3(rng.RandfRange(-halfExtents.X, halfExtents.X), rng.RandfRange(-halfExtents.Y, halfExtents.Y), 0);

        asteroid.LinearVelocity = new Vector3(0, 0, -speed);

        float x = rng.RandfRange(rotationSpeedRange.X, rotationSpeedRange.Y);
        float y = rng.RandfRange(rotationSpeedRange.X, rotationSpeedRange.Y);
        float z = rng.RandfRange(rotationSpeedRange.X, rotationSpeedRange.Y);
        asteroid.AngularVelocity = new Vector3(x, y, z);

        int i = rng.RandiRange(0, possibleShapes.Length - 1);
        asteroid.SetAsteroid(possibleShapes[i]);
        asteroid.Died += OnAsteroidDeath;
    }

    private void OnAsteroidDeath(AsteroidBase asteroid)
    {
        GameValues.Instance.score += 1;
        AsteroidDied?.Invoke(asteroid);
    }
}
