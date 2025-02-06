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

    public void Fire()
	{
		debris.Emitting = true;
		fire.Emitting = true;
		sound.Play();
        GetTree().CreateTimer(0.1).Timeout += () => smoke.Emitting = true;
        GetTree().CreateTimer(2.5).Timeout += OnTimeout;
	}

    private void OnTimeout()
    {
        QueueFree();
    }
}
