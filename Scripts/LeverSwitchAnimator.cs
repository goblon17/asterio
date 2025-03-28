using Godot;
using System;

public partial class LeverSwitchAnimator : Node3D
{
	[Export]
	private Vector2 angleRanges;
	[Export]
	private float duration;

    private bool state = false;

    public override void _Process(double delta)
    {
        float speed = Mathf.Abs(angleRanges.Y - angleRanges.X) / duration;
        Vector3 rotDegs = RotationDegrees;
        rotDegs.Z = Mathf.MoveToward(rotDegs.Z, state ? angleRanges.Y : angleRanges.X, speed * (float)delta);
        RotationDegrees = rotDegs;
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventKey keyEvent && keyEvent.PhysicalKeycode == Key.Shift)
        {
            state = keyEvent.Pressed;
        }
    }
}
