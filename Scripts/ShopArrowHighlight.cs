using Godot;
using System;

public partial class ShopArrowHighlight : MeshInstance3D
{
	[Export]
	private bool highlightOnPress = false;
    [Export]
    private float fadeDuration;
    [Export(hint: PropertyHint.Range, "0, 1")]
    private float toOneDurationPercentage;

    private bool highlighted = false;

    private StandardMaterial3D material;

	public override void _Ready()
	{
		material = Mesh.SurfaceGetMaterial(0).Duplicate() as StandardMaterial3D;
        MaterialOverride = material;
        highlighted = !highlightOnPress;
	}

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventKey keyEvent)
		{
			if (keyEvent.PhysicalKeycode == Key.Shift)
			{
				highlighted = !(highlightOnPress ^ keyEvent.Pressed);
			}
		}
    }

    public override void _Process(double delta)
	{
        float target = highlighted ? 16 : 0;
        float speed = 15 / (fadeDuration * (1 - toOneDurationPercentage));
        if (material.EmissionEnergyMultiplier < 1)
        {
            speed = 1 / (fadeDuration * toOneDurationPercentage);
        }
        material.EmissionEnergyMultiplier = Mathf.MoveToward(material.EmissionEnergyMultiplier, target, speed * (float)delta);
    }
}
