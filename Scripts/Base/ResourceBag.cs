using Godot;
using System;

public partial class ResourceBag : Node
{
    public static PackedScene BulletScene;
    public static PackedScene EnemyShipScene;

    public static PackedScene InfoLabelScene;

    public override void _Ready()
    {
        BulletScene = ResourceLoader.Load<PackedScene>("res://Scenes/Misc/Bullet.tscn");

        EnemyShipScene = ResourceLoader.Load<PackedScene>("res://Scenes/Characters/Enemies/Ship.tscn");

        InfoLabelScene = ResourceLoader.Load<PackedScene>("res://Scenes/Hud/InfoLabel.tscn");
    }
}
