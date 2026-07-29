using Godot;
using System;

public partial class InfoPage : Panel
{
    Button BackButton;
    Button TutorialButton;

    public override void _Ready()
    {
        GetNode<Label>("Version").Text = ProjectSettings.GetSetting("application/config/version").ToString();

        BackButton = GetNode<Button>("BackButton");
        BackButton.Pressed += Hide;

        TutorialButton = GetNode<Button>("TutorialButton");
        TutorialButton.Pressed += AddInitialPage;
    }
    private void AddInitialPage()
    {
        Control InitialPage = GD.Load<PackedScene>("res://Scenes/Hud/InitialPage.tscn").Instantiate<Control>();
        InitialPage.Modulate = Colors.Transparent;
        GetParent().AddChild(InitialPage);

        Tween T = CreateTween();
        T.TweenProperty(InitialPage, Control.PropertyName.Modulate.ToString(), Colors.White, 0.3f);
    }
}