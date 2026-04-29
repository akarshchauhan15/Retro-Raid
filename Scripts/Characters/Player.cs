using System;
using Godot;
using Godot.Collections;

public partial class Player : CharacterBody2D
{
    [Signal]
    public delegate void ScoreChangedEventHandler();
    [Signal]
    public delegate void HealthChangedEventHandler();
    [Signal]
    public delegate void ShieldChangedEventHandler();
    [Signal]
    public delegate void ShotsFiredEventHandler();
    [Signal]
    public delegate void PickupedEventHandler(int Type);

    Camera2D Camera;
    Area2D AutoAimZone;
    Sprite2D Texture;
    GpuParticles2D Particles;
    public Timer CooldownTimer;
    Marker2D BulletSpawnLocation;

    public float Fuel = 100f;
    public int Health = 3;
    public int Shield = 1;
    public int Score = 0;

    Vector2 MaxVelocity = new(700, 500);
    float MinVelocityY = 250;
    Vector2 Acceleration = new(2500, 400);
    Vector2 Friction = new(1600, 2400);

    public override void _Ready()
    {
        Camera = GetNode<Camera2D>("%Camera");
        AutoAimZone = GetNode<Area2D>("AutoAimZone");
        Texture = GetNode<Sprite2D>("Sprite2D");
        Particles = GetNode<GpuParticles2D>("Particles");
        CooldownTimer = GetNode<Timer>("CooldownTimer");
        BulletSpawnLocation = GetNode<Marker2D>("BulletSpawnLocation");
    }
    public override void _Process(double delta)
    {
        AlignCamera();
        if (!Playground.isPlaying)  return;
        CheckMovement(delta);
        UpdateStats(delta);
    }
    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event.IsActionPressed("Shoot") && CooldownTimer.TimeLeft == 0)
            Shoot();
    }
    public void OnHit()
    {   
        if (Shield > 0)
        {
            Shield-=1;
            EmitSignal(SignalName.ShieldChanged);
            return;
        }
        Health-=1;
        EmitSignal(SignalName.HealthChanged);
    }
    public void AddScore(int Value)
    {
        Score += Value;
        EmitSignal(SignalName.ScoreChanged);
    }
    private void CheckMovement(double delta)
    {
        Vector2 Direction;

        Direction = Input.GetVector("Left", "Right", "Up", "Down");
        
        float HorizontalVelocityModifier = (Playground.SliderSpeed + 700) / 1200;

        if (Direction.X != 0)
            Velocity = Velocity.MoveToward(new Vector2(Direction.X  * MaxVelocity.X * HorizontalVelocityModifier, Velocity.Y),Acceleration.X *  (float) delta);
        else
            Velocity = Velocity.MoveToward(new Vector2(0, Velocity.Y), Friction.X * (float) delta);

        if (Direction.Y < 0)
            Playground.SliderSpeed = Mathf.MoveToward(Playground.SliderSpeed, MaxVelocity.Y, Mathf.Abs(Direction.Y) * Acceleration.Y * (float)delta);
        else if (Direction.Y > 0)
            Playground.SliderSpeed = Mathf.MoveToward(Playground.SliderSpeed, MinVelocityY, Mathf.Abs(Direction.Y) * Acceleration.Y * (float)delta);

        Texture.Rotation = Velocity.X / (Playground.SliderSpeed + 400) * 0.2f;

        Particles.AmountRatio = 0.6f + (Playground.SliderSpeed - 250) / 1000;

        MoveAndSlide();
    }
    private void AlignCamera()
    {
        float NewPositionX = 640 + (GlobalPosition.X - 640) * 0.1f;
        Camera.GlobalPosition = new(NewPositionX, 360 - GlobalPosition.Y / 10);
    }
    private void Shoot()
    {
        CooldownTimer.Start();
        EmitSignal(SignalName.ShotsFired);

        Bullet NewBullet = ResourceBag.BulletScene.Instantiate<Bullet>();

        GetNode<Node2D>("%InGameSpawnedObjects/Projectiles").AddChild(NewBullet);
        Array<Area2D> EnemiesInRange = AutoAimZone.GetOverlappingAreas();

        if (EnemiesInRange.Count > 0)     
            NewBullet.Direction = BulletSpawnLocation.GlobalPosition.DirectionTo(EnemiesInRange[EnemiesInRange.Count - 1].GlobalPosition);
        else
            NewBullet.Direction = new Vector2(Velocity.X * 0.0001f, -1).Normalized();

        NewBullet.SetCollisionMaskValue(1, false);
        NewBullet.SetCollisionMaskValue(2, true);

        NewBullet.Speed += Mathf.Abs(Playground.SliderSpeed);
        NewBullet.GlobalPosition = BulletSpawnLocation.GlobalPosition;  
    }
    private void UpdateStats(double delta)
    {
        float HorizontalVelocityModifier = (Playground.SliderSpeed + 700) / 1200;
        Fuel -= 2 * HorizontalVelocityModifier * (float) delta;
    }
}