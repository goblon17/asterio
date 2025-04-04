using Godot;
using System;

[GlobalClass]
public partial class LightControler : MeshInstance3D
{
    [Export]
    private Gradient gradient;
    [Export]
    private Vector2 emissionEnergyRange;
    [Export]
    private float duration;

    public bool turnedOn;

    private float colorOffset;

	public override void _Ready()
	{
        MaterialOverride = Mesh.SurfaceGetMaterial(0).Duplicate() as Material;
        StandardMaterial3D material = MaterialOverride as StandardMaterial3D;
        turnedOn = false;
        colorOffset = 0;
        material.AlbedoColor = gradient.Sample(colorOffset);
        material.EmissionEnergyMultiplier = emissionEnergyRange.X;
    }

    public override void _Process(double delta)
    {
        StandardMaterial3D material = MaterialOverride as StandardMaterial3D;
        
        float colorSpeed = 1 / duration;
        colorOffset += (turnedOn ? 1 : -1) * colorSpeed * (float)delta;
        colorOffset = Math.Clamp(colorOffset, 0, 1);
        material.AlbedoColor = gradient.Sample(colorOffset);

        float energySpeed = Mathf.Abs(emissionEnergyRange.Y - emissionEnergyRange.X) / duration;
        float targetEnergy = turnedOn ? emissionEnergyRange.Y : emissionEnergyRange.X;
        material.EmissionEnergyMultiplier = Mathf.MoveToward(material.EmissionEnergyMultiplier, targetEnergy, energySpeed * (float)delta);
    }
}
