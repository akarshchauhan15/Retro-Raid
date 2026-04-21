using Godot;
using System;

public partial class Helicopter : Enemies
{
    PathFollow2D PathFollow;
    float Speed = 140;

    public override void _Ready()
    {
        base._Ready();

        PathFollow = GetParent<PathFollow2D>();
        Projectiles = GetNodeOrNull("../../../../Pickables");
        Pickables = GetNodeOrNull("../../../../Projectiles");

        Speed *= (GD.RandRange(0,1) - 0.5f) * 2f;
        if (Speed > 0) Rotate((float)Math.PI);
    }
    public override void _PhysicsProcess(double delta)
    {
        PathFollow.Progress += Speed * (float)delta;
    }
}
