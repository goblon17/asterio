using Godot;
using System;

public partial class AsteroidSpawner : Node3D
{
	[Export]
	private double cooldown;
    [Export]
    private float speed;
    [Export]
    private Vector2 halfExtents;
	[Export]
	private PackedScene asteroidPrefab;

    private RandomNumberGenerator rng = new RandomNumberGenerator();

    private double timer = 0;

    public override void _Process(double delta)
    {
        if ((timer += delta) < cooldown)
        {
            return;
        }
        timer = 0;

        Asteroid asteroid = asteroidPrefab.Instantiate<Asteroid>();
        AddChild(asteroid, forceReadableName: true);
        asteroid.Position = new Vector3(rng.RandfRange(-halfExtents.X, halfExtents.X), rng.RandfRange(-halfExtents.Y, halfExtents.Y), 0);
        asteroid.LinearVelocity = new Vector3(0, 0, -speed);
    }
}
