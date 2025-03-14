using Godot;
using System;

public partial class ShopPanel : Node3D
{
    public enum ShopState { Closing, Closed, Opening, Opened }

    [Export]
    private AnimationPlayer animationPlayer;
    [Export]
    private float openAnimationDuration;
    [Export]
    private float closeAnimationDuration;
    [Export]
    public UpgradeIndicator DamageUpgrade { get; private set; }
    [Export]
    public UpgradeIndicator HeatPerShotUpgrade { get; private set; }
    [Export]
    public UpgradeIndicator FireRateUpgrade { get; private set; }
    [Export]
    public UpgradeIndicator HeatReductionUpgrade { get; private set; }

    public ShopState State { get; private set; }

    private PlayerStats playerStats;

    private int availablePoints;

    public override void _Ready()
    {
        animationPlayer.AnimationFinished += OnAnimationFinished;
        playerStats = PlayerStats.Instance;
        playerStats.LevelIncreased += OnPlayerLevelIncreased;
        playerStats.LevelProgressChanged += OnLevelProgressChanged;
        availablePoints = playerStats.CurrentLevelPoints;
    }

    private void OnPlayerLevelIncreased()
    {
        availablePoints++;
        UpdatePointsDisplay();
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseButton)
        {
            if (mouseButton.ButtonIndex == MouseButton.Right)
            {
                if (mouseButton.Pressed)
                {
                    ShowPanel();
                }
                else
                {
                    HidePanel();
                }
            }
        }
    }

    private void ShowPanel()
    {
        State = ShopState.Opening;
        animationPlayer.Play("Move", customSpeed: 1 / openAnimationDuration);
    }

    private void HidePanel()
    {
        State = ShopState.Closing;
        animationPlayer.Play("Move", customSpeed: -1 / closeAnimationDuration, fromEnd: true);
    }

    private void OnAnimationFinished(StringName animName)
    {
        State = State == ShopState.Opening ? ShopState.Opened : ShopState.Closed;


    }

    public bool TryGetPoint()
    {
        if (availablePoints > 0)
        {
            availablePoints--;
            UpdatePointsDisplay();
            return true;
        }
        return false;
    }

    public void ReturnPoint()
    {
        if (availablePoints < playerStats.CurrentLevelPoints)
        {
            availablePoints++;
            UpdatePointsDisplay();
        }
    }

    private void UpdatePointsDisplay()
    {

    }

    private void OnLevelProgressChanged()
    {

    }
}
