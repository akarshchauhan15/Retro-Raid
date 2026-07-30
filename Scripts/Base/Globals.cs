using System;
using Godot;
using Godot.Collections;

public class GameConstants
{
    public enum ScoreEnum { ShipHit, HelicopterHit, TankHit, JetHit, BridgeHit, LevelCompleted}
    public static Dictionary<ScoreEnum, int> ScoreValues = new Dictionary<ScoreEnum, int> {
        {ScoreEnum.ShipHit, 20},
        {ScoreEnum.HelicopterHit,  25},
        {ScoreEnum.TankHit, 15},
        {ScoreEnum.JetHit, 30},
        {ScoreEnum.BridgeHit, 40},
        {ScoreEnum.LevelCompleted, 30},
    };
}

public class ScoreEntry
{
    public int Score { get; set; }
    public DateTime Date { get; set; }

    public ScoreEntry(int score)
    {
        Score = score;
        Date = DateTime.Now;
    }
}