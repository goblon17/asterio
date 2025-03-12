using Godot;
using System;

public partial class PlayerStats : Node3D
{
    public static PlayerStats Instance { get; private set; }

    [Export]
    private PlayerStat fireRate;
    [Export]
    private PlayerStat damage;
    [Export]
    public float health;
    [Export]
    private PlayerStat heatPerShot;
    [Export]
    private PlayerStat heatReductionSpeed;

    public float FireRate => fireRate.GetStatValue(0, 0);
    public float Damage => damage.GetStatValue(0, 0);
    public float HeatPerShot => heatPerShot.GetStatValue(0, 0);
    public float HeatReductionSpeed => heatReductionSpeed.GetStatValue(0, 0);

    public override void _EnterTree()
    {
        Instance = this;
    }

    public override void _ExitTree()
    {
        Instance = null;
    }
}
