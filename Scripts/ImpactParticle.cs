using Godot;
using System;

public partial class ImpactParticle : Node3D
{
	[Export]
	private GpuParticles3D smoke;
	[Export]
	private GpuParticles3D debris;
	[Export]
	private AudioStreamPlayer3D player;
	
	public void Start()
	{
		debris.Emitting = true;
		player.Play();
		GetTree().CreateTimer(0.05).Timeout += () => smoke.Emitting = true;
		smoke.Finished += () => QueueFree();
	}
}
