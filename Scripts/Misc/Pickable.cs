using Godot;
using System;
using System.ComponentModel.DataAnnotations;

public partial class Pickable : Area2D
{
    public enum PickableType { Fuel, Health, Shield }

    public PickableType Type { get; set; }

    public override void _Ready()
    {
        BodyEntered += OnPlayerEntered;

        Tween tween = CreateTween();
        tween.SetParallel();
        tween.TweenProperty(this, Node2D.PropertyName.Modulate.ToString(), Colors.White, 0.2f).From(Colors.Transparent);
        tween.TweenProperty(this, Node2D.PropertyName.Scale.ToString(), Vector2.One, 0.2f).From(Vector2.One * 0.4f).SetTrans(Tween.TransitionType.Quad);
    }
    public virtual void Initialize(PickableType Type)
    { 
        this.Type = Type;
        GetNode<Sprite2D>("Sprite2D").Frame = (int)Type;
    }
    public void OnPlayerEntered(Node2D Body)
    {    
        if (!(Body is Player)) return;
        QueueFree();

        ApplyEffect(Body);
    }
    public void ApplyEffect(Node2D Body)
    {
        Player Player = Body as Player;
        Player.EmitSignal(Player.SignalName.Pickuped, (int)Type);

        switch (Type) 
        {
            case PickableType.Fuel:
                Player.Fuel = Math.Min(Player.Fuel + 40, 100);
                return;

            case PickableType.Health:
                Player.Health = Math.Min(Player.Health + 1, 3);
                Player.EmitSignal(Player.SignalName.HealthChanged);
                return;
            
            case PickableType.Shield:
                Player.Shield = Math.Min(Player.Shield + 1, 3);
                Player.EmitSignal(Player.SignalName.ShieldChanged);
                return;
        }
    }
}
