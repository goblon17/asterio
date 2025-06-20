using Godot;
using System;
using System.Linq;

public partial class StandardAsteroid : AsteroidBase
{
    [Export]
    private PackedScene impactParticle;

    protected override void TakeDamageInternal(float dmg, Vector3 pos, Vector3 normal)
    {
        ImpactParticle particle = SpawnLocalObject<ImpactParticle>(impactParticle, pos, normal);
        particle.Start();
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
