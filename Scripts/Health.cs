using Godot;
using System;

public partial class Health : Area3D
{
    public static Health Instance { get; private set; }

    public event Action<float> HealthChanged;
    public float CurrentHealth
    {
        get => currentHealth;
        set
        {
            currentHealth = value;
            HealthChanged?.Invoke(value);
        }
    }

	[Export]
	private float currentHealth;

    public override void _Ready()
    {
        Instance = this;
        CurrentHealth = PlayerStats.Instance.health;
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
        CurrentHealth -= asteroid.Damage;
        asteroid.Die();
    }
}
