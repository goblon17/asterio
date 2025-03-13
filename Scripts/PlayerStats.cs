using Godot;
using System;
using System.Linq;

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

    public float FireRate => fireRate.GetStatValue(0, shopPanel.FireRateUpgrade.CurrentLevel);
    public float Damage => damage.GetStatValue(0, shopPanel.DamageUpgrade.CurrentLevel);
    public float HeatPerShot => heatPerShot.GetStatValue(0, shopPanel.HeatPerShotUpgrade.CurrentLevel);
    public float HeatReductionSpeed => heatReductionSpeed.GetStatValue(0, shopPanel.HeatReductionUpgrade.CurrentLevel);

    private ShopPanel shopPanel;

    public override void _Ready()
    {
        shopPanel = GetParent().GetChildren().Where(e => e is ShopPanel).Single() as ShopPanel;
    }

    public override void _EnterTree()
    {
        Instance = this;
    }

    public override void _ExitTree()
    {
        Instance = null;
    }
}
