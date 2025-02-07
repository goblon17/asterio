using Godot;
using System;

public partial class HeatPanel : MeshInstance3D
{
	[Export]
	private Gradient gradient;
	[Export]
	private MeshInstance3D heatFill;

    public override void _Ready()
    {
        CallDeferred(nameof(OnReady));
    }

    private void OnReady()
    {
        SetValue(Player.Instance.CurrentHeat);
        Player.Instance.HeatChanged += SetValue;
    }

    public void SetValue(float v)
	{
        Node3D parent = heatFill.GetParentNode3D();
        Vector3 scale = parent.Scale;
		scale.X = v;
		parent.Scale = scale;

        if (heatFill.MaterialOverride is StandardMaterial3D material)
        {
			Color color = gradient.Sample(Mathf.Clamp(v, 0, 1));
            material.AlbedoColor = color;
			material.Emission = color;
        }
    }
}
