using Godot;
using System;

public partial class Enemies : Area2D
{
    Marker2D BulletSpawnLocation;
    Timer CooldownTimer;
    Sprite2D TurretSprite;
    Player Player;

    public Node Projectiles;
    public Node Pickables;

    [Export]
    GameConstants.ScoreEnum EnemySpecificScoreEnum;
    [Export]
    float DetectionRadius = 200.0f;
    [Export]
    float ShootingTimePeriod = 2.0f;
    [Export]
    float FireInaccuracy = 0.2f;
    [Export]
    bool Flying = false;
    public Pickable.PickableType? SpawnPickableOnFree = null;

    public override void _Ready()
    {
        BulletSpawnLocation = GetNode<Marker2D>("BulletSpawnLocation");
        CooldownTimer = GetNode<Timer>("CooldownTimer");
        TurretSprite = GetNode<Sprite2D>("Turret");
        Player = GetTree().Root.GetNode<Player>("Main/Playground/Player");

        if (this is not Helicopter){
            Projectiles = GetNodeOrNull("../../Pickables");
            Pickables = GetNodeOrNull("../../Projectiles");
        }

        BodyEntered += OnCollisionWithPlayer;
        GetNode<Area2D>("PlayerDetectionArea").BodyEntered += (Node2D Body) => { CallDeferred(Enemies.MethodName.Shoot); CooldownTimer.Start(); };
        GetNode<Area2D>("PlayerDetectionArea").BodyExited += (Node2D Body) => { CooldownTimer.Stop(); };
        CooldownTimer.Timeout += Shoot;
        GetNode<VisibleOnScreenNotifier2D>("VisibleOnScreenNotifier2D").ScreenExited += QueueFree;

        CooldownTimer.WaitTime = ShootingTimePeriod;

        CircleShape2D Circle = new CircleShape2D { Radius = DetectionRadius };

        GetNode<CollisionShape2D>("PlayerDetectionArea/CollisionShape2D").Shape = Circle;

        GetNode<VisibleOnScreenNotifier2D>("VisibleOnScreenNotifier2D").Position = new Vector2(0, -DetectionRadius);

        SetCollisionMaskValue(1, Flying);
    }
    public override void _PhysicsProcess(double delta)
    {
        if (CooldownTimer.TimeLeft > 0)
        {
            TurretSprite.LookAt(Player.GlobalPosition);
            TurretSprite.GlobalRotationDegrees += 180;
        }
    }
    public void OnHit()
    {
        QueueFree();
        Player.AddScore(GameConstants.ScoreValues[EnemySpecificScoreEnum]);
        if (SpawnPickableOnFree == null) return;
        AddPickable();
    }
    private void AddPickable()
    {
        Pickable NewPickable = ResourceBag.PickableScene.Instantiate<Pickable>();
        NewPickable.Initialize(SpawnPickableOnFree.Value);

        Pickables.CallDeferred(Node2D.MethodName.AddChild.ToString(), NewPickable);
        NewPickable.SetDeferred(Node2D.PropertyName.GlobalPosition, GlobalPosition);
    }
    private void OnCollisionWithPlayer(Node2D Body)
    {
        if (!(Body is Player)) return;
        Player.OnHit();
        OnHit();
    }
    private void Shoot()
    {
        Bullet NewBullet = ResourceBag.BulletScene.Instantiate<Bullet>();

        float Inaccuracy = (float) GD.RandRange(-1d, 1d) * FireInaccuracy;

        NewBullet.Direction = BulletSpawnLocation.GlobalPosition.DirectionTo(Player.GlobalPosition).Rotated(Inaccuracy);
        NewBullet.SetCollisionMaskValue(2, false);

        Projectiles.AddChild(NewBullet);
        NewBullet.GlobalPosition = BulletSpawnLocation.GlobalPosition;
    }
}