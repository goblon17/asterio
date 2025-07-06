using Godot;
using System;
using System.Linq;

public partial class SilverAsteroid : AsteroidBase
{
    [Export]
    private PackedScene impactParticle;
    [Export]
    private AudioStreamPlayer3D audioPlayer;
    [Export]
    private float damageThreshold;

    protected override bool TakeDamageInternal(float dmg, Vector3 pos, Vector3 normal)
    {
        if (dmg >= damageThreshold)
        {
            ImpactParticle particle = SpawnLocalObject<ImpactParticle>(impactParticle, pos, normal);
            particle.Start();
            return true;
        }
        else
        {
            audioPlayer.Play();
            return false;
        }
    }

    protected override void DieInternal()
    {
        Node parent = GetParent();
        foreach (ImpactParticle particle in GetChildren().OfType<ImpactParticle>())
        {
            particle.Reparent(parent);
        }
    }
}
