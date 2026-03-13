using Godot;
using System;

public partial class InfoLabel : Label
{
    Timer KillTimer;
    public override void _Ready()
    {
        KillTimer = GetNode<Timer>("Timer");
        KillTimer.Timeout += Kill;
    }
    private void Kill()
    {
        Tween tween = CreateTween();
        tween.TweenProperty(this, "modulate:a", 0, 0.6f);
        tween.TweenCallback(Callable.From(() => QueueFree()));
    }
}
