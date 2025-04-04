using Godot;
using System;
using System.Linq;

public partial class HealCounter : MeshInstance3D
{
	[Export]
	private UpgradeIndicator[] upgradeIndicators;
	[Export]
	private LightControler[] lightControlers;

	public override void _Process(double delta)
	{
		int level = upgradeIndicators.Count(e => e.CurrentLevel > 0);

		int count = lightControlers.Length;
		for (int i = 0; i < count; i++)
		{
			lightControlers[i].turnedOn = i < level;
		}
	}
}
