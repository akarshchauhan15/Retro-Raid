using Godot;
using System;
using System.Threading.Tasks;

public partial class BaseLevel : Node2D
{

    [Export]
    public bool JetsEnabled = false;
    [Export]
    public float FireInaccuracyMultiplier = 1;
    [Export]
    public float FireSpeedMultiplier = 1;

    Area2D FinishLine;
    GameOverlay Overlay;
    Playground Playground;

    public override void _Ready()
    {
        Playground = GetNode<Playground>("../../../");

        FinishLine = GetNode<Area2D>("FinishLine");
        FinishLine.BodyEntered += EndLevel;

        Overlay = GetTree().Root.GetNode<GameOverlay>("Main/HUD/GameOverlay");
    }
    private async void EndLevel(Node2D Body)
    {       
        Player.DisableMovement = true;
        Player.CheckForCollisions = false;
        Tween tween = CreateTween();

        tween.SetParallel(true);

        tween.TweenProperty(Body, Node2D.PropertyName.Position.ToString(), new Vector2(Body.Position.X, -200), 2f).SetTrans(Tween.TransitionType.Quad);
        tween.TweenMethod(Callable.From<float>((Value) => Playground.SliderSpeed = Value), Playground.SliderSpeed, 160f, 1.6f);

        tween.SetParallel(false);

        string PromptOnEnd = Playground.CurrentLevel >= Playground.TotalLevel ? "Completed" : "Finish";
        await Overlay.Prompt(PromptOnEnd, true, true);

        ((Player)Body).AddScore(GameConstants.ScoreValues[GameConstants.ScoreEnum.LevelCompleted]);
        ResetLevel();
    }
    private async Task ResetLevel()
    {   
        Playground.GetNode<AnimationPlayer>("AnimationPlayer").Play("SetBelow");

        Playground.GetNode<Node2D>("InGameSpawnedObjects").Position = Vector2.Zero;
        Playground.CleanUp(false);

        PackedScene Centre = BaseMapDefaults.ModularLevelScenes[(int)BaseMapDefaults.ModularLevelNamesEnum.Centre];

        for (int i = 0; i < 3; i++)
        {
            BaseMapComponent InitialCentre = Centre.Instantiate<BaseMapComponent>();
            GetParent<Node2D>().AddChild(InitialCentre);
            InitialCentre.GlobalPosition = Vector2.Up * i * 720;
        }

        Playground.CurrentLevel += 1;
        if (Playground.CurrentLevel > Playground.TotalLevel) { EndCampaign(); return; }

        Playground.AddLevel();

        Playground.SliderSpeed = 300.0f;
        Overlay.Anim.Play("Unfade");
        Playground.Anim.Play("Enter");
        
        await ToSignal(Overlay.Anim, AnimationPlayer.SignalName.AnimationFinished); 

        Overlay.Prompt($"Level {Playground.CurrentLevel}");
        Playground.isPlaying = true;
        Player.DisableMovement = false;
        Player.CheckForCollisions = true;

        QueueFree();
    }
    private void EndCampaign()
    {
        Playground.ForceEndGame();
        Playground.GetNode<DeadMenu>("../HUD/DeadMenu").ResetGame();

        Overlay.Anim.Play("Unfade");

        QueueFree();
    }
}