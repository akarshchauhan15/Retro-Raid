using System;
using System.Threading.Tasks;
using Godot;

public partial class Playground : Node2D
{
    [Signal] public delegate void GameStartedEventHandler();

    Node2D Slider;
    Node2D LevelContainer;
    Node2D EnemyContainer;
    PackedScene NextMapPackedComponent;
    public AnimationPlayer Anim;

    Random Random = new();
    public static float SliderSpeed = 300.0f;
    public static bool isPlaying = false;

    public enum GameModes {Campaign, Zen}
    public static GameModes CurrentGameMode;
    public static int CurrentLevel = 1;

    public override void _Ready()
    {
        Slider = GetNode<Node2D>("InGameSpawnedObjects");
        LevelContainer = GetNode<Node2D>("InGameSpawnedObjects/LevelContainer");
        EnemyContainer = GetNode<Node2D>("InGameSpawnedObjects/Enemies");
        Anim = GetNode<AnimationPlayer>("AnimationPlayer");

        GetNode<Timer>("Timers/JetSpawnTimer").Timeout += SpawnEnemyJets;

        NextMapPackedComponent = BaseMapDefaults.ModularLevelScenes[0];
    }
    public override void _Process(double delta)
    {
        if (isPlaying)
        Slider.GlobalPosition += Vector2.Down * SliderSpeed * (float) delta;
    }
    public async Task InitialStart()
    {
        EmitSignal(Playground.SignalName.GameStarted);

        isPlaying = true;
        Tween tween = CreateTween();
        tween.TweenMethod(Callable.From<float>(Value => SliderSpeed = Value), 0.0f, 300.0f, 0.6f);

        if (CurrentGameMode == GameModes.Zen) GetNode<Timer>("Timers/JetSpawnTimer").Start();

        Anim.Play("Fly");
        await ToSignal(Anim, AnimationPlayer.SignalName.AnimationFinished);

        Player.DisableMovement = false;
    }
    public void AddLevel()
    {
        BaseLevel LevelScene = ResourceLoader.Load<PackedScene>($"res://Scenes/Level/Level{CurrentLevel}.tscn").Instantiate<BaseLevel>();
        LevelContainer.AddChild(LevelScene);
        LevelScene.GlobalPosition = Vector2.Up * 720 * 3;
        
        SpawnFixedPresetEnemy("Ship", LevelScene.GetNode<Node2D>("SpawnPositions/Ship"));
        SpawnFixedPresetEnemy("Tank", LevelScene.GetNode<Node2D>("SpawnPositions/Tank"));

        if (CurrentLevel >= 3) GetNode<Timer>("Timers/JetSpawnTimer").Start();
    }
    public void SpawnModularMapComponent(Vector2 SacrificedPosition)
    {
        BaseMapComponent MapComponent = NextMapPackedComponent.Instantiate<BaseMapComponent>();
        
        MapComponent.Position = new Vector2(0, SacrificedPosition.Y - 720 * 3);
        LevelContainer.AddChild(MapComponent);
    
        Enemies Ship = SpawnPresetEnemy("Ship", MapComponent);

        if (Random.Next(0, 10) < 3){ 
            Enemies Tank = SpawnPresetEnemy("Tank", MapComponent);
            if (Ship.Position.X > Tank.Position.X) Tank.Rotate(2 * (float)Math.PI);
        }

        Path2D? HelicopterPath = MapComponent.GetNodeOrNull<Path2D>($"SpawnPositions/Helicopter/Path2D");
        if (HelicopterPath != null){
            Enemies Helicopter = ResourceBag.EnemyScenes["Helicopter"].Instantiate<Enemies>();
            PathFollow2D PathFollow = new();

            Path2D HelicopterPathNew = HelicopterPath.Duplicate() as Path2D;
            EnemyContainer.AddChild(HelicopterPathNew);
            HelicopterPathNew.GlobalPosition = HelicopterPath.GlobalPosition;
            HelicopterPathNew.AddChild(PathFollow);
            PathFollow.AddChild(Helicopter);
        }

        BaseMapDefaults.ModularLevelNamesEnum NextMapEnum = MapComponent.NextModularLevels.PickRandom();
        NextMapPackedComponent =  BaseMapDefaults.ModularLevelScenes[(int)NextMapEnum];
    }
    private Enemies SpawnPresetEnemy(string EnemyType, BaseMapComponent MapComponent)
    {
        Enemies Enemy = ResourceBag.EnemyScenes[EnemyType].Instantiate<Enemies>();
        Node SpawnPostions = MapComponent.GetNode<Node2D>($"SpawnPositions/{EnemyType}");
        Vector2 RefPosition = (Vector2)SpawnPostions.GetChildren().PickRandom().Get(Node2D.PropertyName.GlobalPosition);
        
        int RandomInt = Random.Next(0,10);
        if (RandomInt < 3)
        Enemy.SpawnPickableOnFree = (Pickable.PickableType) RandomInt; 
        
        EnemyContainer.AddChild(Enemy);
        Enemy.GlobalPosition = RefPosition;
        return Enemy;
    }
    private void SpawnFixedPresetEnemy(string EnemyType, Node2D MarkerContainer)
    {
        foreach (Marker2D Marker in MarkerContainer.GetChildren())
        {
            Enemies Enemy = ResourceBag.EnemyScenes[EnemyType].Instantiate<Enemies>();
            EnemyContainer.AddChild(Enemy);
            Enemy.GlobalPosition = Marker.GlobalPosition;
        }
    }
    private void SpawnEnemyJets()
    {
        int PositionX = 320 * Random.Next(1, 4) - 160;

        for (int i=2; i>0; i--)
        {
            Enemies EnemyJet = ResourceBag.EnemyScenes["Jet"].Instantiate<Enemies>();
            EnemyContainer.AddChild(EnemyJet);
            EnemyJet.GlobalPosition = new Vector2(PositionX, -20);
            PositionX += 320;
        }
    }
}