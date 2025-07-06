using Godot;
using System;
using System.Collections.Generic;

[GlobalClass]
public partial class WaveConfig : Resource
{
    [Export]
    private AsteroidSpawnFrequency[] spawnFrequencies;
    [Export]
    private float spawnDelay;
    [Export]
    private float waveDuration;

    private Dictionary<AsteroidType, float> spawnFrequenciesDict;

    public IReadOnlyDictionary<AsteroidType, float> SpawnFrequencies
    {
        get
        {
            if (spawnFrequenciesDict == null)
            {
                spawnFrequenciesDict = new Dictionary<AsteroidType, float>();
                foreach (AsteroidSpawnFrequency spawnFrequency in spawnFrequencies)
                {
                    spawnFrequenciesDict[spawnFrequency.Type] = spawnFrequency.Frequency;
                }
            }
            return spawnFrequenciesDict;
        }
    }
    public float SpawnDelay => spawnDelay;
    public float WaveDuration => waveDuration;
}
