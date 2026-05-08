using Godot;
using Godot.Collections;
using System;

public partial class ResourceBag : Node
{
    public static PackedScene BulletScene;

    public static Dictionary<string, PackedScene> EnemyScenes = new Dictionary<string, PackedScene>();

    public static PackedScene PickableScene;

    public static PackedScene InfoLabelScene;

    public override void _Ready()
    {
        BulletScene = ResourceLoader.Load<PackedScene>("res://Scenes/Misc/Bullet.tscn");

        string[] EnemyNames = ["Ship", "Tank", "Helicopter", "Jet"];
        foreach (string EnemyName in EnemyNames)
            EnemyScenes.Add(EnemyName, ResourceLoader.Load<PackedScene>($"res://Scenes/Characters/Enemies/{EnemyName}.tscn"));

        PickableScene = ResourceLoader.Load<PackedScene>("res://Scenes/Misc/Pickable.tscn");

        InfoLabelScene = ResourceLoader.Load<PackedScene>("res://Scenes/Hud/InfoLabel.tscn");
    }
}
