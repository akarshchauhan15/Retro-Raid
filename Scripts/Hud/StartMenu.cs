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

    public override void _Ready()
    {
        ButtonContainer = GetNode<VBoxContainer>("ButtonContainer");
        GamemodeContainer = GetNode<VBoxContainer>("GamemodeContainer");

        StartButton = GetNode<Button>("ButtonContainer/StartButton");
        OptionsButton = GetNode<Button>("ButtonContainer/OptionsButton");
        ExitButton = GetNode<Button>("ButtonContainer/HBoxContainer/ExitButton");
        BackButton = GetNode<Button>("GamemodeContainer/BackButton");

        ExitPanel = GetNode<Panel>("../ExitPanel");

        StartButton.Pressed += SwitchContainers;
        BackButton.Pressed += SwitchContainers;
        ExitButton.Pressed += ExitPanel.Show;

        GamemodeContainer.GetNode<Button>("Zen").Pressed += PlayZen;
    }
    private void SwitchContainers()
    {
        ButtonContainer.Visible = !ButtonContainer.Visible;
        GamemodeContainer.Visible = !GamemodeContainer.Visible;
    }
    private void PlayCampaign()
    {
        Hide();
        GetNode<AnimationPlayer>("../GameOverlay/AnimationPlayer").Play("OverlayAppear");
        GetTree().Root.GetNode<Playground>("Main/Playground").InitialStart();
    }
    private void PlayZen()
    {
        Hide();
        GetNode<AnimationPlayer>("../GameOverlay/AnimationPlayer").Play("OverlayAppear");
        GetTree().Root.GetNode<Playground>("Main/Playground").InitialStart();
    }
}