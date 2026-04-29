using Godot;
using System;

public partial class StartMenu : Control
{
    Button StartButton;
    Button OptionsButton;
    Button ExitButton;

    Panel ExitPanel;

    public override void _Ready()
    {
        StartButton = GetNode<Button>("ButtonContainer/StartButton");
        OptionsButton = GetNode<Button>("ButtonContainer/OptionsButton");
        ExitButton = GetNode<Button>("ButtonContainer/HBoxContainer/ExitButton");

        ExitPanel = GetNode<Panel>("../ExitPanel");

        StartButton.Pressed += StartGame;
        ExitButton.Pressed += ExitPanel.Show;
    }
    private void StartGame()
    {
        Hide();
        GetNode<AnimationPlayer>("../GameOverlay/AnimationPlayer").Play("OverlayAppear");
        GetTree().Root.GetNode<Playground>("Main/Playground").InitialStart();
    }
}