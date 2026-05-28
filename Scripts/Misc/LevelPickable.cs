using Godot;
using System;

public partial class LevelPickable : Pickable
{
    public override void _Ready()
    {
        BodyEntered += OnPlayerEntered;
    }
    public override void Initialize(PickableType Type)
    {
        this.Type = Type;
        GetNode<Sprite2D>("Sprite2D").Frame = (int)Type / 2;
    }
    public void OnPlayerEntered(Node2D Body)
    {
        if (!(Body is Player)) return;

        Tween tween = CreateTween();
        tween.SetParallel();

        tween.TweenProperty(this, Node2D.PropertyName.Modulate.ToString(), Colors.Transparent, 0.3f);
        tween.TweenProperty(this, Node2D.PropertyName.Scale.ToString(), Vector2.One * 1.2f, 0.3f);

        tween.SetParallel(false);

        tween.TweenCallback(Callable.From(QueueFree));

        ApplyEffect(Body);
    }
}