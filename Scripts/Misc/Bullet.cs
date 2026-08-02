using Godot;
using System;

public partial class Bullet : Area2D
{
    public float Speed = 400.0f;
    public Vector2 Direction = Vector2.Down;

    public override void _Ready()
    {
        GetNode<Timer>("Timer").Timeout += Fall;
        AreaEntered += OnCollision;
        BodyEntered += OnCollision;
    }
    public override void _Process(double delta)
    {
        Position += Direction * Speed * (float) delta;
        Speed -= 100 * (float) delta;
    }
    private void Fall()
    {
        Tween T = CreateTween();
        Sprite2D S = GetNode<Sprite2D>("Sprite2D");

        T.TweenProperty(S, Sprite2D.PropertyName.Scale.ToString(), Vector2.One * 0.1f, 1f).SetTrans(Tween.TransitionType.Cubic);
        T.TweenCallback(Callable.From(QueueFree)).SetDelay(0.5f);
    }
    private void OnCollision(Node2D Body)
    {
        QueueFree();
        if (Body.HasMethod("OnHit")) Body.Call("OnHit");
    }
}
