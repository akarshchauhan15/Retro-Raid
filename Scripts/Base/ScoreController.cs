using Godot;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.Json;

public partial class ScoreController : Node
{
    public static List<ScoreEntry> EndlessScores = new List<ScoreEntry>();
    public static List<ScoreEntry> CampaignScores = new List<ScoreEntry>();
    public static string Path = "res://scores.json";

    public static void AddScores(int Score, Playground.GameModes GameMode)
    {   
        switch (GameMode)
        {
            case Playground.GameModes.Campaign:
                CampaignScores.Add(new ScoreEntry(Score));
                CampaignScores = CampaignScores.OrderByDescending(entry => entry.Score).Take(10).ToList();
                break;

            case Playground.GameModes.Zen:
                EndlessScores.Add(new ScoreEntry(Score));
                EndlessScores = EndlessScores.OrderByDescending(entry => entry.Score).Take(10).ToList();
                break;
        }

        SaveScores();
    }
    public static void LoadScores()
    {
        if (!FileAccess.FileExists(Path))
            return;
        
        FileAccess File = FileAccess.Open(Path, FileAccess.ModeFlags.Read);
        string Json = File.GetAsText();
        File.Close();

        var CombinedScores = JsonSerializer.Deserialize<Tuple<List<ScoreEntry>, List<ScoreEntry>>>(Json);
        CampaignScores = CombinedScores.Item1;
        EndlessScores = CombinedScores.Item2;
    }
    public static Tuple<string, string> GetScoreListString(List<ScoreEntry> ScoreList)
    {   
        if (ScoreList.Count == 0)
            return Tuple.Create("", "");

        string ScoreListText = "";
        string DateListText = "";

        DateTime Today = DateTime.Now;

        foreach (ScoreEntry Score in ScoreList)
        {
            string DisplayDate;
            int Difference = Today.Day - Score.Date.Day;

            switch (Difference)
            {
                case 0:
                    DisplayDate = "Today";
                    break;
                case 1:
                    DisplayDate = "Yesterday";
                    break;
                default:
                    DisplayDate = Score.Date.ToString("dd MMM", new CultureInfo("en-GB"));
                    break;
            }

            ScoreListText += $"{Score.Score}\n";
            DateListText += $"{DisplayDate}\n";
        }

        return Tuple.Create(ScoreListText, DateListText);
    }
    private static void SaveScores()
    {
        Tuple<List<ScoreEntry>, List<ScoreEntry>> CombinedScore = new Tuple<List<ScoreEntry>, List<ScoreEntry>>(CampaignScores, EndlessScores);
        var Json = JsonSerializer.Serialize(CombinedScore);
        FileAccess File = FileAccess.Open(Path, FileAccess.ModeFlags.Write);
        File.StoreString(Json);
        File.Close();
    }
}