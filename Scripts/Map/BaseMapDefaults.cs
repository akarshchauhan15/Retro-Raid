using Godot;
using Godot.Collections;
using System;

public partial class BaseMapDefaults : Node
{
    public static PackedScene AirportScene;
    public static readonly Array<PackedScene> ModularLevelScenes = [];

    public enum ModularLevelNamesEnum
    {
        Centre,
        CentreToRight,
        CentreToLeft,
        CentreIsland,
        Right,
        RightToCentre,
        RightIsland,
        Left,
        LeftToCentre,
        LeftIsland,
    }
    public override void _Ready()
    {
        foreach (string Address in Enum.GetNames(typeof(ModularLevelNamesEnum)))
        {
            ModularLevelScenes.Add(ResourceLoader.Load<PackedScene>($"res://Scenes/Map/{Address}.tscn"));
        }

        AirportScene = ResourceLoader.Load<PackedScene>("res://Scenes/Map/Airport.tscn");
    }
}