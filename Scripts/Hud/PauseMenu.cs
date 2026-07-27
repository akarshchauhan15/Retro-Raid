using Godot;
using System;

public partial class PauseMenu : Panel
{
    Button ResumeButton;

    Button OptionsButton;
    Button ScoresButton;
    Button InfoButton;

    Button MainMenuButton;
    Button ExitButton;

    public override void _Ready()
    {
        ResumeButton = GetNode<Button>("ButtonContainer/ResumeButton");

        OptionsButton = GetNode<Button>("ButtonContainer/Preferences/OptionsButton");
        ScoresButton = GetNode<Button>("ButtonContainer/Preferences/ScoresButton");
        InfoButton = GetNode<Button>("ButtonContainer/Preferences/InfoButton");

        MainMenuButton = GetNode<Button>("ButtonContainer/Quit/MainMenuButton");
        ExitButton = GetNode<Button>("ButtonContainer/Quit/ExitButton");

        ResumeButton.Pressed += ToggleGameState;

        InfoButton.Pressed += () => GetNode<Panel>("../InfoPage").Show();

        MainMenuButton.Pressed += OnMenuButtonPressed;
        ExitButton.Pressed += () => GetNode<Panel>("../ExitPanel").Show();
    }
    public void ToggleGameState()
    {
        GetTree().Paused = !GetTree().Paused;
        Visible = !Visible;
        GetNode<TextureButton>("../MenuSlide/ColorRect/PauseButton").SetPressedNoSignal(false);
    }
    public void OnMenuButtonPressed()
    {
        ToggleGameState();
        Playground P = GetTree().Root.GetNode<Playground>("Main/Playground");
        P.ForceEndGame();

        GetNode<DeadMenu>("../DeadMenu").ResetGame();
    }
}