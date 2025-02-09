using Godot;
using System;

public partial class CannonParticles : Node3D
{
	[Export]
	private GpuParticles3D debris;
    [Export]
    private GpuParticles3D fire;
    [Export]
    private GpuParticles3D smoke;
	[Export]
	private AudioStreamPlayer3D sound;
    [Export]
    private Vector2 pitchScaleRange;

    private RandomNumberGenerator rng = new RandomNumberGenerator();

    private bool smokeFinished = false;
    private bool soundFinished = false;

    public void Fire()
	{
		debris.Emitting = true;
		fire.Emitting = true;
        sound.PitchScale = rng.RandfRange(pitchScaleRange.X, pitchScaleRange.Y);
        smoke.Finished += () =>
        {
            smokeFinished = true;
            TryDestroy();
        };
        sound.Finished += () =>
        {
            soundFinished = true;
            TryDestroy();
        };
		sound.Play();
        GetTree().CreateTimer(0.1).Timeout += () => smoke.Emitting = true;
	}

    private void TryDestroy()
    {
        if (!smokeFinished || !soundFinished)
        {
            return;
        }

        QueueFree();
    }
}
