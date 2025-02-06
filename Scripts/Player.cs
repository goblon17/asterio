using Godot;
using System;
using System.Collections.Generic;

public partial class Player : Node3D
{
    [Export]
    private Vector2 sensitivity;
    [Export]
    private Vector2 angleRange;
	[Export]
	private Cannon[] cannons;
    [Export]
    private RayCast3D rayCast;

    private bool mouseButtonDown = false;
    private double timer = 0;
    private int currentCannonIndex;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        Input.MouseMode = Input.MouseModeEnum.Captured;
	}

    public override void _Process(double delta)
    {
        if (timer > 0)
        {
            timer -= delta;
        }

        if (mouseButtonDown && timer <= 0)
        {
            Fire();
        }
    }

    private void Fire()
    {
        cannons[currentCannonIndex].Fire();
        currentCannonIndex = Mathf.PosMod(currentCannonIndex + 1, cannons.Length);
        timer = 1 / PlayerStats.Instance.fireRate;

        rayCast.ForceRaycastUpdate();
        if (rayCast.IsColliding() && rayCast.GetCollider() is Asteroid asteroid)
        {
            asteroid.TakeDamage(PlayerStats.Instance.damage, rayCast.GetCollisionPoint(), rayCast.GetCollisionNormal());
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

            case InputEventKey key:
                if (key.Pressed && key.Keycode == Key.Escape)
                {
                    GetTree().Quit();
                }
                break;
        }
    }
}
