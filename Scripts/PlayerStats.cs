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

    [Export]
    private int initialLevelPoints;
    [Export]
    private int requiredAsteroidsForLevel;
    [Export]
    private int requiredAsteroidsForLevelIncrease;

    public event Action LevelIncreased;
    public event Action LevelProgressChanged;

    public float FireRate => fireRate.GetStatValue(0, shopPanel.FireRateUpgrade.CurrentLevel);
    public float Damage => damage.GetStatValue(0, shopPanel.DamageUpgrade.CurrentLevel);
    public float HeatPerShot => heatPerShot.GetStatValue(0, shopPanel.HeatPerShotUpgrade.CurrentLevel);
    public float HeatReductionSpeed => heatReductionSpeed.GetStatValue(0, shopPanel.HeatReductionUpgrade.CurrentLevel);

    public float LevelProgressPrecent => (float)destroyedAsteroids / CurrentLevelRequirement;

    public int CurrentLevelPoints => currentLevel + initialLevelPoints;

    private int CurrentLevelRequirement => requiredAsteroidsForLevel + requiredAsteroidsForLevelIncrease * currentLevel;

    private ShopPanel shopPanel;

    private int currentLevel;
    private int destroyedAsteroids;

    public override void _Ready()
    {
        shopPanel = GetParent().GetChildren().OfType<ShopPanel>().Single();
        destroyedAsteroids = 0;
        currentLevel = 0;
        AsteroidSpawner.Instance.AsteroidDied += OnAsteroidDeath;
    }

    private void OnAsteroidDeath(Asteroid asteroid)
    {
        destroyedAsteroids++;
        if (destroyedAsteroids >= CurrentLevelRequirement)
        {
            currentLevel++;
            destroyedAsteroids = 0;
            LevelIncreased?.Invoke();
        }
        LevelProgressChanged?.Invoke();
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
