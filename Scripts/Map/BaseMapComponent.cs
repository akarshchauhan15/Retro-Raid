using Godot;
using Godot.Collections;


public partial class BaseMapComponent : Node2D
{
    public VisibleOnScreenNotifier2D Notifier2D;

    [Export]
    public Array<BaseMapDefaults.ModularLevelNamesEnum> NextModularLevels = [];

    public override void _Ready()
    {
        Notifier2D = GetNode<VisibleOnScreenNotifier2D>("VisibleOnScreenNotifier2D");
        Notifier2D.ScreenExited += OnExitedFromScreen;
    }
    private void OnExitedFromScreen()
    {   
        QueueFree();
        
        if (Playground.CurrentGameMode == Playground.GameModes.Zen) 
            GetTree().Root.GetNode<Playground>("Main/Playground").SpawnModularMapComponent(Position);
    }
}
