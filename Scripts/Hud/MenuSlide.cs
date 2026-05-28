using Godot;
using System;

public partial class MenuSlide : Slide
{
    TextureButton FullscreenButton;
    TextureButton PauseButton;
    TextureButton ExitButton;

    bool ExitPressedOnce = false;
    public override void _Ready()
    {
        base._Ready();

        FullscreenButton = GetNode<TextureButton>("ColorRect/FullscreenButton");
        PauseButton = GetNode<TextureButton>("ColorRect/PauseButton");
        ExitButton = GetNode<TextureButton>("ColorRect/ExitButton");

        FullscreenButton.Toggled += OnFullscreenButtonToggled;
        PauseButton.Toggled += OnPauseButtonToggled;
        ExitButton.Pressed += OnExitButtonPressed;

        
        GetNode<Playground>("/root/Main/Playground").GameStateChanged += (bool GameStarted) => PauseButton.Visible = GameStarted; 
        MotionCompleted += (bool Hidden) => { if (Hidden) ExitPressedOnce = false; };
    }
    private void OnFullscreenButtonToggled(bool Fullscreen)
    {
        DisplayServer.WindowSetMode(Fullscreen ? DisplayServer.WindowMode.Fullscreen : DisplayServer.WindowMode.Windowed);
    }
    private void OnPauseButtonToggled(bool Paused)
    {
        Control P = GetNode<PauseMenu>("../PauseMenu");
        GetTree().Paused = !GetTree().Paused;
        P.Visible = !P.Visible;
    }
    private void OnExitButtonPressed()
    {
        if (ExitPressedOnce) { GetTree().Quit(); return; }
        ExitPressedOnce = true;
    }
}
