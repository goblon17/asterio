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

    public override void _Ready()
    {
        base._Ready();

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
			//todo: heal
		}
    }
}
