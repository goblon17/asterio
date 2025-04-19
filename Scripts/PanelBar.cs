using Godot;
using System;

public partial class PanelBar : MeshInstance3D
{
	[Export]
	private Node3D anchor;

	public float Value
	{
		get => anchor.Scale.Y;
		set
		{
            Vector3 scale = anchor.Scale;
            scale.Y = Mathf.Clamp(value, 0, 1);
			anchor.Scale = scale;
        }
	}
}
