using Godot;
using System;
using System.Linq;

public partial class HealButton : LightControler
{
	[Export]
	private UpgradeIndicator[] upgradeIndicators;
	[Export]
	private TextMesh labelMesh;
	[Export]
	private Key healKey;

	private ShopPanel shopPanel;

    private T FindParentOfType<T>() where T : Node
    {
        Node parent = GetParent();
        while (parent != null)
        {
            if (parent is T)
            {
                return parent as T;
            }
            parent = parent.GetParent();
        }
        return null;
    }

    public override void _Ready()
    {
        base._Ready();

		shopPanel = FindParentOfType<ShopPanel>();

		labelMesh.Text = OS.GetKeycodeString(DisplayServer.KeyboardGetKeycodeFromPhysical(healKey));
    }

    public override void _Process(double delta)
	{
		turnedOn = upgradeIndicators.Count(e => e.CurrentLevel > 0) >= upgradeIndicators.Length;

		base._Process(delta);
	}

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventKey inputKey && inputKey.Pressed && inputKey.PhysicalKeycode == healKey)
		{
			if (turnedOn && 
				(shopPanel.State == ShopPanel.ShopState.Opened || shopPanel.State == ShopPanel.ShopState.Opening))
			{
				Heal();
			}
		}
    }

	private void Heal()
	{
        foreach (UpgradeIndicator upgradeIndicator in upgradeIndicators)
		{
			upgradeIndicator.DecreaseLevel(false);
		}

		Health.Instance.CurrentHealth = PlayerStats.Instance.health;
	}
}
