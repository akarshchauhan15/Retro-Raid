using Godot;
using System;
using System.Threading.Tasks;

public partial class StartMenu : Control
{
    VBoxContainer ButtonContainer;
    VBoxContainer GamemodeContainer;

    Button StartButton;
    Button OptionsButton;
    Button ExitButton;
    Button BackButton;

    GameOverlay Overlay;

    Panel ExitPanel;

    Playground Playground;
    AnimationPlayer Anim;

    public override void _Ready()
    {
        ButtonContainer = GetNode<VBoxContainer>("ButtonContainer");
        GamemodeContainer = GetNode<VBoxContainer>("GamemodeContainer");

        StartButton = GetNode<Button>("ButtonContainer/StartButton");
        OptionsButton = GetNode<Button>("ButtonContainer/OptionsButton");
        ExitButton = GetNode<Button>("ButtonContainer/HBoxContainer/ExitButton");
        BackButton = GetNode<Button>("GamemodeContainer/BackButton");

        Overlay = GetNode<GameOverlay>("../GameOverlay");

        ExitPanel = GetNode<Panel>("../ExitPanel");

        Playground = GetTree().Root.GetNode<Playground>("Main/Playground");
        Anim = GetNode<AnimationPlayer>("../GameOverlay/AnimationPlayer");

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
    private async void PlayCampaign()
    {
        Playground.CurrentGameMode = Playground.GameModes.Campaign;
        Hide();
        Anim.Play("OverlayAppear");
        Playground.InitialStart();
        Playground.AddLevel();

        await ToSignal(Anim, AnimationPlayer.SignalName.AnimationFinished);
        Overlay.Prompt($"Level {Playground.CurrentLevel}", Wait: true);
    }
    private async void PlayZen()
    {
        Playground.CurrentGameMode = Playground.GameModes.Zen;
        Hide();
        Anim.Play("OverlayAppear");
        Playground.InitialStart();

        await ToSignal(Anim, AnimationPlayer.SignalName.AnimationFinished);
        Overlay.Prompt("Endless", Wait: true);
    }
}