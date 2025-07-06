using Godot;
using System;

public partial class AsteroidSpawner : Node3D
{
    public static AsteroidSpawner Instance { get; private set; }

    [Export]
    private float speed;
    [Export]
    private Vector2 rotationSpeedRange;
    [Export]
    private Vector2 halfExtents;
	[Export]
	private PackedScene standardAsteroidPrefab;
    [Export]
    private PackedScene goldAsteroidPrefab;
    [Export]
    private PackedScene silverAsteroidPrefab;
    [Export]
    private AsteroidShape[] possibleShapes;
    [Export]
    private WaveConfig[] waves;

    public event Action<AsteroidBase> AsteroidDied;

    private RandomNumberGenerator rng = new RandomNumberGenerator();

    private double asteroidTimer = 0;
    private double waveTimer  = 0;

    private int currentWaveIndex = 0;
    private WaveConfig currentWave = null;

    public override void _EnterTree()
    {
        Instance = this;
    }

    public override void _ExitTree()
    {
        Instance = null;
    }

    public override void _Ready()
    {
        currentWave = waves[currentWaveIndex];
    }

    public override void _Process(double delta)
    {
        if ((waveTimer += delta) >= currentWave.WaveDuration)
        {
            waveTimer = 0;
            currentWaveIndex++;
            currentWave = waves[Mathf.Min(currentWaveIndex, waves.Length)];
        }

        if ((asteroidTimer += delta) < currentWave.SpawnDelay)
        {
            return;
        }
        asteroidTimer = 0;

        PackedScene asteroidPrefab = GetRandomAsteroid();
        if (asteroidPrefab == null)
        {
            return;
        }

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

    private PackedScene GetRandomAsteroid()
    {
        float x = rng.Randf();
        foreach ((AsteroidType type, float frequency) in currentWave.SpawnFrequencies)
        {
            x -= frequency;
            if (x <= 0)
            {
                return GetAsteroidPrefab(type);
            }
        }
        return null;
    }

    private PackedScene GetAsteroidPrefab(AsteroidType type)
    {
        return type switch
        {
            AsteroidType.Standard => standardAsteroidPrefab,
            AsteroidType.Gold => goldAsteroidPrefab,
            AsteroidType.Silver => silverAsteroidPrefab,
            _ => null,
        };
    }

    private void OnAsteroidDeath(AsteroidBase asteroid)
    {
        GameValues.Instance.score += 1;
        AsteroidDied?.Invoke(asteroid);
    }
}
