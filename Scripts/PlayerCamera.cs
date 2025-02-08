using Godot;
using System;

public partial class PlayerCamera : Camera3D
{
	[Export]
	private Vector2 shakeRange;
	[Export]
	private double shakeDuration;

	private RandomNumberGenerator rng = new RandomNumberGenerator();

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
}
