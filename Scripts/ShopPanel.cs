using Godot;
using System;

public partial class ShopPanel : Node3D
{
    public enum ShopState { Closing, Closed, Opening, Opened }

    [Export]
    private AnimationPlayer animationPlayer;

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
        animationPlayer.Play("Move");
    }

    private void HidePanel()
    {
        State = ShopState.Closing;
        animationPlayer.PlayBackwards("Move");
    }

    private void OnAnimationFinished(StringName animName)
    {
        State = State == ShopState.Opening ? ShopState.Opened : ShopState.Closed;


    }
}
