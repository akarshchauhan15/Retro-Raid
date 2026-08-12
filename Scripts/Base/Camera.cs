using Godot;
using System;

public partial class Camera : Camera2D
{
    float CurrentShake = 0;
    float Decay = 0;

    public override void _PhysicsProcess(double delta)
    {
        if (CurrentShake > 0)
        {
            CurrentShake = Mathf.Lerp(CurrentShake, 0, (float)delta * Decay);
            Offset = Vector2.FromAngle(Mathf.DegToRad(new Random().Next(0, 360))) * CurrentShake;
        }
    }
    public void InitiateShake(float ShakeStrength = 8, float Speed = 8)
    {
        if (CurrentShake > 0.1) return;
        CurrentShake = ShakeStrength;
        Decay = Speed;
    }
}