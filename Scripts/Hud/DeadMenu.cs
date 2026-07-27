using Godot;
using System;

public partial class DeadMenu : Control
{   
    Label CauseLabel;
    Player Player;

    public override void _Ready()
    {   
        CauseLabel = GetNode<Label>("Cause");

        GetNode<Button>("MainMenuButton").Pressed += ResetGame;

        Player = GetNode<Player>("../../Playground/Player");
        Player.PlayedDied += DeadMenuAppear;
    }
    private void DeadMenuAppear(string Cause)
    {
        switch (Cause)
        {
            case "PlayerHit":
                CauseLabel.Text = "Shot down. Avoid getting hit!";
                break;
            case "FuelDepleted":
                CauseLabel.Text = "Fuel depleted. Keep an eye on fuel gauge!";
                break;
        }
        GetNode<AnimationPlayer>("AnimationPlayer").Play("Appear");
    }
    public void ResetGame()
    {
        Hide();
        GetNode<Playground>("../../Playground").ResetGame();
        GetNode<Control>("../StartMenu").Show();
        GetNode<Control>("../GameOverlay").Hide();
    }
}
