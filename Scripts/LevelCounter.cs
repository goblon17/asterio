using Godot;
using System;

public partial class LevelCounter : MeshInstance3D
{
	[Export]
	private Node3D fillParent;
	[Export]
	private TextMesh levelText;

	private PlayerStats stats;
    private ShopPanel shop;

	public override void _Ready()
	{
		CallDeferred(nameof(OnReady));
	}

	private void OnReady()
	{
        shop = ShopPanel.Instance;
        stats = PlayerStats.Instance;
        stats.LevelProgressChanged += OnLevelProgressChanged;
        shop.PointsDisplayUpdated += OnPointsDisplayUpdated;

        OnLevelProgressChanged();
        OnPointsDisplayUpdated();
    }

    private void OnLevelProgressChanged()
    {
        Vector3 scale = fillParent.Scale;
        scale.Y = Mathf.Max(0.001f, stats.LevelProgressPrecent);
        fillParent.Scale = scale;
    }

    private void OnPointsDisplayUpdated()
    {
        levelText.Text = shop.AvailablePoints.ToString();
    }
}
