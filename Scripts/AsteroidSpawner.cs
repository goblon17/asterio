using Godot;
using System;

public partial class AsteroidSpawner : Node3D
{
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
    private ArrayMesh[] possibleMehses;
    [Export]
    private ConcavePolygonShape3D[] possibleCollisions;

    private RandomNumberGenerator rng = new RandomNumberGenerator();

    private double timer = double.MaxValue;

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

        float x = rng.RandfRange(rotationSpeedRange.X, rotationSpeedRange.Y);
        float y = rng.RandfRange(rotationSpeedRange.X, rotationSpeedRange.Y);
        float z = rng.RandfRange(rotationSpeedRange.X, rotationSpeedRange.Y);
        asteroid.AngularVelocity = new Vector3(x, y, z);
    }
}
