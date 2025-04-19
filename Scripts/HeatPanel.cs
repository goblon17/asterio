using Godot;
using System;

public partial class HeatPanel : MeshInstance3D
{
	[Export]
	private PanelBar damageBar;
    [Export]
    private PanelBar firerateBar;
    [Export]
    private PanelBar heatBar;
    [Export]
    private PanelBar coolBar;

    public override void _Ready()
    {
        damageBar.Value = 0;
        firerateBar.Value = 0;
        heatBar.Value = 0;
        coolBar.Value = 0;

        CallDeferred(nameof(OnReady));
    }

    private void OnReady()
    {
        ShopPanel shopPanel = ShopPanel.Instance;
        shopPanel.DamageUpgrade.CurrentLevelChanged += () => SetValue(damageBar, shopPanel.DamageUpgrade);
        shopPanel.FireRateUpgrade.CurrentLevelChanged += () => SetValue(firerateBar, shopPanel.FireRateUpgrade);
        shopPanel.HeatPerShotUpgrade.CurrentLevelChanged += () => SetValue(heatBar, shopPanel.HeatPerShotUpgrade);
        shopPanel.HeatReductionUpgrade.CurrentLevelChanged += () => SetValue(coolBar, shopPanel.HeatReductionUpgrade);
    }

    private void SetValue(PanelBar bar, UpgradeIndicator indicator)
    {
        bar.Value = indicator.CurrentLevel / 4f;
    }
}
