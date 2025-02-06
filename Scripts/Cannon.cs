using Godot;
using System;

public partial class Cannon : Node3D
{
	[Export]
	private AnimationPlayer animationPlayer;
	[Export]
	private Node3D particlePosition;
	[Export]
	private PackedScene particle;

	public void Fire()
	{
		animationPlayer.Stop();
		animationPlayer.Play("Fire");

		CannonParticles cannonParticles = particle.Instantiate<CannonParticles>();
		particlePosition.AddChild(cannonParticles);
		cannonParticles.Fire();
	}
}
