using Godot;
using System;
using System.Collections.Generic;

public partial class Player : Node3D
{
	[Export]
	private Cannon[] cannons;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseButton)
		{
			if (mouseButton.ButtonIndex == MouseButton.Left && mouseButton.Pressed)
			{
				cannons[0].Fire();
			}
		}
    }
}
