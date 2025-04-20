using Godot;
using System;

public partial class PanelBar : MeshInstance3D
{
	[Export]
	private Node3D anchor;
	[Export]
	private float speed;

	public float Value
	{
		get => target;
		set => target = Mathf.Clamp(value, 0, 1);
	}

	private float ScaleY
	{
		get => anchor.Scale.Y;
		set
		{
            Vector3 scale = anchor.Scale;
            scale.Y = value;
            anchor.Scale = scale;
        }
	}

	private float target = 0;

    public override void _Ready()
    {
		CallDeferred(nameof(OnReady));
    }

	private void OnReady()
	{
		ScaleY = target;
	}

    public override void _Process(double delta)
    {
		ScaleY = Mathf.MoveToward(ScaleY, target, speed * (float)delta);
    }
}
