using Godot;
using System;

public partial class ShopPanel : Node3D
{
    public enum ShopState { Closing, Closed, Opening, Opened }

    [Export]
    private AnimationPlayer animationPlayer;
    [Export]
    private float openAnimationDuration;
    [Export]
    private float closeAnimationDuration;

    public ShopState State { get; private set; }

    public override void _Ready()
    {
        animationPlayer.AnimationFinished += OnAnimationFinished;
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
}
