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
    }
    public void Initialize(PickableType Type)
    { 
        this.Type = Type;
        GetNode<Sprite2D>("Sprite2D").Frame = (int)Type;
    }
    private void OnPlayerEntered(Node2D Body)
    {    
        if (!(Body is Player)) return;
        QueueFree();

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
