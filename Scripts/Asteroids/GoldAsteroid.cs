using Godot;
using System;
using System.Linq;

public partial class GoldAsteroid : AsteroidBase
{
    [Export]
    private PackedScene impactParticle;
    [Export]
    private Color targetColor;
    [Export]
    private float targetEmission;
    [Export]
    private float regenerateCooldown;
    [Export]
    private float regenerateSpeed;

    public override AsteroidType Type => AsteroidType.Gold;

    private StandardMaterial3D material;

    private Color startingColor;
    private float startingEmission;

    private float regenerateTimer;

    public override void _Ready()
    {
        base._Ready();
        material = mesh.GetSurfaceOverrideMaterial(0).Duplicate() as StandardMaterial3D;
        mesh.SetSurfaceOverrideMaterial(0, material);
        startingColor = material.AlbedoColor;
        startingEmission = material.EmissionEnergyMultiplier;
    }

    protected override bool TakeDamageInternal(float dmg, Vector3 pos, Vector3 normal)
    {
        ImpactParticle particle = SpawnLocalObject<ImpactParticle>(impactParticle, pos, normal);
        particle.Start();

        regenerateTimer = 0;
        return true;
    }

    protected override void DieInternal()
    {
        Node parent = GetParent();
        foreach (ImpactParticle particle in GetChildren().OfType<ImpactParticle>())
        {
            particle.Reparent(parent);
        }
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        
        float deltaF = (float)delta;
        if ((regenerateTimer += deltaF) > regenerateCooldown)
        {
            CurrentHealth += regenerateSpeed * deltaF;
        }

        UpdateMaterial();
    }

    private void UpdateMaterial()
    {
        float t = Mathf.Clamp(CurrentHealth / MaxHealth, 0, 1);
        material.AlbedoColor = startingColor.Lerp(targetColor, t);
        material.EmissionEnergyMultiplier = Mathf.Lerp(startingEmission, targetEmission, t);
    }
}
