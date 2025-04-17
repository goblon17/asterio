using Godot;
using System;

public partial class ShopPanel : Node3D
{
    public enum ShopState { Closing, Closed, Opening, Opened }

    public static ShopPanel Instance { get; private set; }

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
    [Export]
    private TextMesh pointsCount;
    [Export]
    private Node3D pointsBar;

    public event Action PointsDisplayUpdated;

    public ShopState State { get; private set; }

    private PlayerStats playerStats;

    public int AvailablePoints { get; private set; }

    public override void _EnterTree()
    {
        Instance = this;
    }

    public override void _ExitTree()
    {
        Instance = null;
    }

    public override void _Ready()
    {
        animationPlayer.AnimationFinished += OnAnimationFinished;
        playerStats = PlayerStats.Instance;
        playerStats.LevelIncreased += OnPlayerLevelIncreased;
        playerStats.LevelProgressChanged += UpdateLevelProgress;
        AvailablePoints = playerStats.CurrentLevelPoints;
        UpdatePointsDisplay();
        UpdateLevelProgress();
    }

    private void OnPlayerLevelIncreased()
    {
        AvailablePoints++;
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
        if (AvailablePoints > 0)
        {
            AvailablePoints--;
            UpdatePointsDisplay();
            return true;
        }
        return false;
    }

    public void ReturnPoint()
    {
        if (AvailablePoints < playerStats.CurrentLevelPoints)
        {
            AvailablePoints++;
            UpdatePointsDisplay();
        }
    }

    private void UpdatePointsDisplay()
    {
        pointsCount.Text = AvailablePoints.ToString();
        PointsDisplayUpdated?.Invoke();
    }

    private void UpdateLevelProgress()
    {
        Vector3 scale = pointsBar.Scale;
        scale.X = Mathf.Max(0.001f, playerStats.LevelProgressPrecent);
        pointsBar.Scale = scale;
    }
}
