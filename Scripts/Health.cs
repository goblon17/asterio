using Godot;
using System;

public partial class Health : Area3D
{
	[Export]
	private float currentHealth;

    public override void _Ready()
    {
        currentHealth = PlayerStats.Instance.health;
        BodyEntered += OnBodyEntered;
    }

    private void OnBodyEntered(Node3D body)
    {
        if (body is Asteroid asteroid)
        {
            OnAsteroidHit(asteroid);
        }
    }

    private void OnAsteroidHit(Asteroid asteroid)
    {
        currentHealth -= asteroid.Damage;
        GD.Print("bulkech");
        asteroid.Die();
    }
}
