using Godot;
using System;

[GlobalClass]
public partial class PlayerStat : Resource
{
    [Export]
    private Curve valueByUpgrade;
    [Export]
    private float curveWidth;

    public float GetStatValue(int level, int upgrade)
    {
        return valueByUpgrade.Sample((level + upgrade) / curveWidth);
    }
}
