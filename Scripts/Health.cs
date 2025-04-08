using Godot;
using System;

public partial class Health : Area3D
{
    public delegate void HealthChangedEvent(float current, float old);

    public static Health Instance { get; private set; }

    public event HealthChangedEvent HealthChanged;
    public float CurrentHealth
    {
        get => currentHealth;
        set
        {
            float old = currentHealth;
            currentHealth = value;
            HealthChanged?.Invoke(value, old);
        }
    }

    [Export]
    private AudioStreamPlayer3D[] audio;

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
        foreach (var a in audio)
        {
            a.Play();
        }
        asteroid.Die();
    }
}
