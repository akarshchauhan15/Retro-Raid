using Godot;
using Godot.Collections;

public partial class GameOverlay : Control
{
    Player Player;

    ProgressBar FuelMeter;
    ProgressBar HealthMeter;
    ProgressBar CooldownMeter;
    HBoxContainer Shields;
    VBoxContainer EventContainer;
    Label ScoreLabel;

    public override void _Ready()
    {
        Player = GetTree().Root.GetNode<Player>("Main/Playground/Player");

        FuelMeter = GetNode<ProgressBar>("Bar/FuelMeter");
        HealthMeter = GetNode<ProgressBar>("Bar/HealthMeter");
        CooldownMeter = GetNode<ProgressBar>("Bar/CooldownMeter");
        Shields = GetNode<HBoxContainer>("Bar/Shields");
        EventContainer = GetNode<VBoxContainer>("EventContainer");

        ScoreLabel = GetNode<Label>("Bar/ScoreLabel");

        Player.ScoreChanged += () => ScoreLabel.Text = Player.Score.ToString();
        Player.HealthChanged += () => HealthMeter.Value = Player.Health;
        Player.ShieldChanged += UpdateShields;
        Player.ShotsFired += StartCooldown;
        Player.Pickuped += AddEventHappened;
    }
    public override void _Process(double delta)
    {
        FuelMeter.Value = Player.Fuel;
    }
    private void StartCooldown()
    {
        Tween tween = CreateTween();
        tween.TweenProperty(CooldownMeter, ProgressBar.PropertyName.Value.ToString(), 100, Player.CooldownTimer.WaitTime).From(0).SetEase(Tween.EaseType.InOut).SetTrans(Tween.TransitionType.Quad);
    }
    private void UpdateShields()
    {
        for (int i = 0; i<Player.Shield; i++) ((Control)Shields.GetChild(i)).Show();
        for (int i = Player.Shield; i<3; i++) ((Control)Shields.GetChild(i)).Hide();
    }
    private void AddEventHappened(int Type)
    {
        InfoLabel Label = ResourceBag.InfoLabelScene.Instantiate<InfoLabel>();

        Label.Text = $"Picked up {(Pickable.PickableType)Type}";
        EventContainer.AddChild(Label);
    }
}