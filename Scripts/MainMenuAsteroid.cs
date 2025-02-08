using Godot;
using System;

public partial class MainMenuAsteroid : Node3D
{
	[Export]
	private MeshInstance3D mesh;

	public Timer timer;
	
	private float speed = 0;
	private Vector3 rotation = Vector3.Zero;

	public override void _Process(double delta)
	{
		RotationDegrees += rotation * (float)delta;
		Position += Vector3.Forward * speed * (float)delta;
	}

	public void Init(ArrayMesh mesh, Vector3 rotation, float speed)
	{
		this.mesh.Mesh = mesh;
		this.rotation = rotation;
		this.speed = speed;
	}

    public override void _ExitTree()
    {
		if (timer != null)
		{
			timer.Stop();
		}
    }
}
