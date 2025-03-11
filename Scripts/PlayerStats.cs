using Godot;
using System;

public partial class PlayerStats : Node3D
{
    public static PlayerStats Instance { get; private set; }

    [Export]
    public double fireRate;
    [Export]
    public float damage;
    [Export]
    public float health;
    [Export]
    public float heatPerShot;
    [Export]
    public float heatReductionSpeed;

    public override void _EnterTree()
    {
        Instance = this;
    }

    public override void _ExitTree()
    {
        Instance = null;
    }
}
