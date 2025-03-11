using Godot;
using System;

public partial class ShopPanel : Node3D
{
    public enum ShopState { Closing, Closed, Opening, Opened }

    [Export]
    private AnimationPlayer animationPlayer;

    public ShopState State { get; private set; }

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
        animationPlayer.AnimationFinished -= OnPanelHidden;
        animationPlayer.Play("Move");
        animationPlayer.AnimationFinished += OnPanelShown;

        State = ShopState.Opening;
    }

    private void OnPanelShown(StringName _)
    {
        animationPlayer.AnimationFinished -= OnPanelShown;

        State = ShopState.Opened;
    }

    private void HidePanel()
    {
        animationPlayer.AnimationFinished -= OnPanelShown;
        animationPlayer.PlayBackwards("Move");
        animationPlayer.AnimationFinished += OnPanelHidden;

        State = ShopState.Closing;
    }

    private void OnPanelHidden(StringName _)
    {
        animationPlayer.AnimationFinished -= OnPanelHidden;

        State = ShopState.Closed;
    }
}
