using Godot;
using System;

[GlobalClass]
public partial class AsteroidSpawnFrequency : Resource
{
    [Export]
    private AsteroidType asteroidType;
    [Export]
    private float frequency;

    public AsteroidType Type => asteroidType;
    public float Frequency => frequency;
}
