using Godot;
using System;
using System.Collections.Generic;

public partial class ScoresMenu : Control
{
    public override void _Ready()
    {
        GetNode<Button>("BackButton").Pressed += BackButtonPressed;
        SetScoreList();
    }
    public void SetScoreList()
    {
        ScoreController.LoadScores();
        List<Tuple<string, string>> ScoreList = [];
        ScoreList.Add(ScoreController.GetScoreListString(ScoreController.CampaignScores));
        ScoreList.Add(ScoreController.GetScoreListString(ScoreController.EndlessScores));

        int Count = 0;
        foreach (Control Tab in GetNode<Control>("Control/TabContainer").GetChildren())
        {
            Tuple<string, string> Score = ScoreList[Count];
            Tab.GetNode<Label>("ScoreList").Text = Score.Item1;
            Tab.GetNode<Label>("DateList").Text = Score.Item2;

            Tab.GetNode<Label>("NoScorePrompt").Visible = Score.Item1 == "";
            Count++;
        } 
    }
    private void BackButtonPressed()
    {
        Hide();
        GetNode<Control>("../StartMenu").Show();
    }
    
}