using Godot;
using Godot.Collections;

public class GameConstants
{
    public enum ScoreEnum { ShipHit, HelicopterHit, TankHit, BridgeHit }
    public static Dictionary<ScoreEnum, int> ScoreValues = new Dictionary<ScoreEnum, int> {
        {ScoreEnum.ShipHit, 20},
        {ScoreEnum.HelicopterHit,  15},
        {ScoreEnum.TankHit, 15},
        {ScoreEnum.BridgeHit, 40},
    };
}