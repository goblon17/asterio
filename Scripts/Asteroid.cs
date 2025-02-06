using Godot;
using System;
using System.Linq;

public partial class Asteroid : RigidBody3D
{
	[Export]
	private float health;
	[Export]
	private float damage;
	[Export]
	private MeshInstance3D mesh;
	[Export]
	private CollisionShape3D collisionShape;
	[Export]
	private PackedScene impactParticle;

	public float Damage => damage;

    public override void _Ready()
    {
		Scale = Vector3.One * 0.01f;
		Tween tween = CreateTween();
		tween.TweenProperty(this, Node3D.PropertyName.Scale.ToString(), Vector3.One, 1);
    }

    public void TakeDamage(float dmg, Vector3 pos, Vector3 normal)
	{
		ImpactParticle particle = impactParticle.Instantiate<ImpactParticle>();
		AddChild(particle);
		particle.GlobalPosition = pos;
		particle.Quaternion = new Quaternion(Vector3.Up, normal);
		particle.Start();

        if ((health -= dmg) <= 0)
        {
            Die();
        }
    }

	public void Die()
	{
        Node parent = GetParent();
        foreach (ImpactParticle particle in GetChildren().OfType<ImpactParticle>())
		{
			particle.Reparent(parent);
		}

		QueueFree();
	}

	public void SetAsteroid(ArrayMesh mesh, ConcavePolygonShape3D collisionShape)
	{
		this.mesh.Mesh = mesh;
		this.collisionShape.Shape = collisionShape;
	}
}
