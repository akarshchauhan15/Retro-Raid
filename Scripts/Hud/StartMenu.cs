using Godot;
using System;

public partial class StartMenu : Control
{
    VBoxContainer ButtonContainer;
    VBoxContainer GamemodeContainer;

    Button StartButton;
    Button OptionsButton;
    Button ExitButton;
    Button BackButton;

    Panel ExitPanel;

    Playground Playground;

    public override void _Ready()
    {
        ButtonContainer = GetNode<VBoxContainer>("ButtonContainer");
        GamemodeContainer = GetNode<VBoxContainer>("GamemodeContainer");

        StartButton = GetNode<Button>("ButtonContainer/StartButton");
        OptionsButton = GetNode<Button>("ButtonContainer/OptionsButton");
        ExitButton = GetNode<Button>("ButtonContainer/HBoxContainer/ExitButton");
        BackButton = GetNode<Button>("GamemodeContainer/BackButton");

        ExitPanel = GetNode<Panel>("../ExitPanel");

        Playground = GetTree().Root.GetNode<Playground>("Main/Playground");

        StartButton.Pressed += SwitchContainers;
        BackButton.Pressed += SwitchContainers;
        ExitButton.Pressed += ExitPanel.Show;

        GamemodeContainer.GetNode<Button>("Campaign").Pressed += PlayCampaign;
        GamemodeContainer.GetNode<Button>("Zen").Pressed += PlayZen;
    }
    private void SwitchContainers()
    {
        ButtonContainer.Visible = !ButtonContainer.Visible;
        GamemodeContainer.Visible = !GamemodeContainer.Visible;
    }
    private void PlayCampaign()
    {
        Playground.CurrentGameMode = Playground.GameModes.Campaign;
        Hide();
        GetNode<AnimationPlayer>("../GameOverlay/AnimationPlayer").Play("OverlayAppear");
        Playground.InitialStart();
        Playground.AddLevel();

    }
    private void PlayZen()
    {
        Playground.CurrentGameMode = Playground.GameModes.Zen;
        Hide();
        GetNode<AnimationPlayer>("../GameOverlay/AnimationPlayer").Play("OverlayAppear");
        Playground.InitialStart();
        Playground.SpawnModularMapComponent(Vector2.Down * 720);
    }
}