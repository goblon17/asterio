using Godot;
using System;

public partial class Heat : MeshInstance3D
{
    [Export]
    private Gradient gradient;
    [Export]
    private Node3D anchor;
    [Export]
    private MeshInstance3D fill;

    public override void _Ready()
    {
        SetValue(0);
        CallDeferred(nameof(OnReady));
    }

    private void OnReady()
    {
        SetValue(Player.Instance.CurrentHeat);
        Player.Instance.HeatChanged += SetValue;
    }

    public void SetValue(float v)
    {
        Vector3 scale = anchor.Scale;
        scale.X = Mathf.Clamp(v, 0.001f, 1);
        anchor.Scale = scale;

        if (fill.MaterialOverride is StandardMaterial3D material)
        {
            Color color = gradient.Sample(Mathf.Clamp(v, 0, 1));
            material.AlbedoColor = color;
            material.Emission = color;
        }
    }
}
