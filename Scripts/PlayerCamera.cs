using Godot;
using System;

public partial class PlayerCamera : Camera3D
{
	[Export]
	private Vector2 shakeRange;
	[Export]
	private double shakeDuration;
	[Export]
	private Vector2 fovRange;
	[Export]
	private float fovAnimationDuration;

	private RandomNumberGenerator rng = new RandomNumberGenerator();

	private bool zoomedIn = false;
	private bool canZoomIn = true;

	public override void _Ready()
	{
		CallDeferred(nameof(OnReady));
	}

	private void OnReady()
	{
        Health.Instance.HealthChanged += (_) => ShakeCamera();
	}

	private void ShakeCamera()
	{
		Tween tween = CreateTween();
		tween.TweenMethod(Callable.From((double time) =>
		{
			float t = 1 - (float)(time / shakeDuration);
			HOffset = rng.RandfRange(-shakeRange.X, shakeRange.X) * t;
			VOffset = rng.RandfRange(-shakeRange.Y, shakeRange.Y) * t;
		}), 0, shakeDuration, shakeDuration);
		tween.TweenCallback(Callable.From(() =>
		{
			HOffset = 0;
			VOffset = 0;
		}));
	}

    public override void _Process(double delta)
    {
        float target = zoomedIn ? fovRange.X : fovRange.Y;
		float speed = Mathf.Abs(fovRange.Y - fovRange.X) / fovAnimationDuration;
		Fov = Mathf.MoveToward(Fov, target, speed * (float)delta);
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseButton)
		{
			if (mouseButton.ButtonIndex == MouseButton.Right)
			{
				canZoomIn = !mouseButton.Pressed;
				zoomedIn = false;
			}

			if (mouseButton.Pressed && canZoomIn)
			{
                if (mouseButton.ButtonIndex == MouseButton.WheelUp)
                {
                    zoomedIn = true;
                }
                else if (mouseButton.ButtonIndex == MouseButton.WheelDown)
                {
                    zoomedIn = false;
                }
            }
		}
    }
}
