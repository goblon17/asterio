using Godot;
using System;

public partial class GameValues : Node
{
	public static GameValues Instance { get; private set; }

	[Export]
	public PackedScene MainMenu { get; private set; }
    [Export]
    public PackedScene Game { get; private set; }
    [Export]
    public PackedScene GameOver { get; private set; }

    public int score;

	public override void _Ready()
	{
		Instance = this;
	}
}
