using Godot;
using System;
using System.Collections.Generic;

public partial class Player : Node3D
{
    public static Player Instance { get; private set; }

    [Export]
    private Vector2 sensitivity;
    [Export]
    private Vector2 angleRange;
	[Export]
	private Cannon[] cannons;
    [Export]
    private RayCast3D rayCast;

    public event Action<float> HeatChanged;

    private bool mouseButtonDown = false;
    private double timer = 0;
    private int currentCannonIndex;

    public float CurrentHeat 
    { 
        get => currentHeat; 
        private set
        {
            currentHeat = value;
            HeatChanged?.Invoke(value);
        }
    }
    private float currentHeat = 0;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        Instance = this;
        Input.MouseMode = Input.MouseModeEnum.Captured;
	}

    public override void _Process(double delta)
    {
        if (timer > 0)
        {
            timer -= delta;
        }

        if (timer <= 0)
        {
            if (CurrentHeat > 0)
            {
                CurrentHeat -= PlayerStats.Instance.HeatReductionSpeed * (float)delta;
                if (CurrentHeat < 0)
                {
                    CurrentHeat = 0;
                }
            }

            if (mouseButtonDown && CurrentHeat < 1 - PlayerStats.Instance.HeatPerShot)
            {
                Fire();
            }
        }
    }

    private void Fire()
    {
        cannons[currentCannonIndex].Fire();
        currentCannonIndex = Mathf.PosMod(currentCannonIndex + 1, cannons.Length);
        timer = 1 / PlayerStats.Instance.FireRate;
        CurrentHeat += PlayerStats.Instance.HeatPerShot;

        rayCast.ForceRaycastUpdate();
        if (rayCast.IsColliding() && rayCast.GetCollider() is Asteroid asteroid)
        {
            asteroid.TakeDamage(PlayerStats.Instance.Damage, rayCast.GetCollisionPoint(), rayCast.GetCollisionNormal());
        }
    }

    public override void _Input(InputEvent @event)
    {
        switch (@event)
        {
            case InputEventMouseMotion mouseMotion:
                Vector3 rotation = RotationDegrees;
                rotation += new Vector3(mouseMotion.Relative.Y * sensitivity.Y, -mouseMotion.Relative.X * sensitivity.X, 0);
                rotation.X = Mathf.Clamp(rotation.X, -angleRange.X, angleRange.X);
                rotation.Y = Mathf.Clamp(rotation.Y, -angleRange.Y, angleRange.Y);
                RotationDegrees = rotation;
                break;

            case InputEventMouseButton mouseButton:
                if (mouseButton.ButtonIndex == MouseButton.Left)
                {
                    mouseButtonDown = mouseButton.Pressed;
                }
                break;
        }
    }
}
