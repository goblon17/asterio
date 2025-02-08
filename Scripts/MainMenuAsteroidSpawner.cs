using Godot;
using Godot.Collections;
using System;

public partial class MainMenuAsteroidSpawner : Node3D
{
    [Export]
    private double cooldown;
    [Export]
    private float speed;
    [Export]
    private Vector2 rotationSpeedRange;
    [Export]
    private Vector2 halfExtents;
    [Export(hint: PropertyHint.Range, "0,1")]
    private double explodeTime;
    [Export]
    private Node3D cannonPosition;
    [Export]
	private PackedScene asteroidPrefab;
    [Export]
    private PackedScene cannonParticles;
    [Export]
    private PackedScene impactParticlas;
	[Export]
	private Array<ArrayMesh> possibleAsteroids;

    private double ExplodeRealTime => Mathf.Abs(GlobalPosition.Z / speed) * explodeTime;

    private RandomNumberGenerator rng = new RandomNumberGenerator();

    private double timer = double.MaxValue;

    private SceneTreeTimer treeTimer = null;
	
	public override void _Process(double delta)
	{
        if ((timer += delta) < cooldown)
        {
            return;
        }
        timer = 0;

        MainMenuAsteroid asteroid = asteroidPrefab.Instantiate<MainMenuAsteroid>();
        AddChild(asteroid, forceReadableName: true);
        asteroid.Position = new Vector3(rng.RandfRange(-halfExtents.X, halfExtents.X), rng.RandfRange(-halfExtents.Y, halfExtents.Y), 0);

        float x = rng.RandfRange(rotationSpeedRange.X, rotationSpeedRange.Y);
        float y = rng.RandfRange(rotationSpeedRange.X, rotationSpeedRange.Y);
        float z = rng.RandfRange(rotationSpeedRange.X, rotationSpeedRange.Y);

        asteroid.Init(possibleAsteroids.PickRandom(), new Vector3(x, y, z), speed);

        asteroid.timer = new Timer();
        asteroid.AddChild(asteroid.timer);
        asteroid.timer.OneShot = true;
        asteroid.timer.Start(ExplodeRealTime);
        asteroid.timer.Timeout += () =>
        {
            CannonParticles cannonParticle = cannonParticles.Instantiate<CannonParticles>();
            cannonPosition.AddChild(cannonParticle);
            cannonParticle.Fire();

            asteroid.timer = new Timer();
            asteroid.AddChild(asteroid.timer);
            asteroid.timer.OneShot = true;
            asteroid.timer.Start(0.1);
            asteroid.timer.Timeout += () =>
            {
                ImpactParticle impactParticle = impactParticlas.Instantiate<ImpactParticle>();
                AddChild(impactParticle);
                impactParticle.Position = asteroid.Position;
                impactParticle.Start();
                asteroid.timer = null;
                asteroid.QueueFree();
            };
        };
    }
}
