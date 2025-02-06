using Godot;
using System;

public partial class PlayerStats : Node3D
{
    public static PlayerStats Instance { get; private set; }

    [Export]
    public double fireRate;

    public override void _Ready()
    {
        Instance = this;
    }
}
