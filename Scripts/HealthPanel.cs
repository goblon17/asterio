using Godot;
using System;

public partial class HealthPanel : MeshInstance3D
{
    [Export]
    private MeshInstance3D healthFill;

    public override void _Ready()
	{
        CallDeferred(nameof(OnReady));
	}

    private void OnReady()
    {
        SetValue(Health.Instance.CurrentHealth / PlayerStats.Instance.health);
        Health.Instance.HealthChanged += OnHealthChanged;
    }

    private void OnHealthChanged(float currentHealth)
    {
        SetValue(currentHealth / PlayerStats.Instance.health);
    }

    public void SetValue(float v)
    {
        Node3D parent = healthFill.GetParentNode3D();
        Vector3 scale = parent.Scale;
        scale.Y = v;
        parent.Scale = scale;
    }
}
