using Godot;
using System;

public partial class MenuSlide : Slide
{
    TextureButton FullscreenButton;
    TextureButton SoundButton;
    TextureButton PauseButton;
    TextureButton ExitButton;

    bool ExitPressedOnce = false;
    public override void _Ready()
    {
        base._Ready();

        FullscreenButton = GetNode<TextureButton>("ColorRect/FullscreenButton");
        SoundButton = GetNode<TextureButton>("ColorRect/SoundButton");
        PauseButton = GetNode<TextureButton>("ColorRect/PauseButton");
        ExitButton = GetNode<TextureButton>("ColorRect/ExitButton");

        FullscreenButton.Toggled += OnFullscreenButtonToggled;
        SoundButton.Toggled += OnSoundButtonToggled;
        PauseButton.Toggled += OnPauseButtonToggled;
        ExitButton.Pressed += OnExitButtonPressed;

        GetNode<Playground>("/root/Main/Playground").GameStateChanged += (bool GameStarted) => PauseButton.Visible = GameStarted; 
        MotionCompleted += (bool Hidden) => { if (Hidden) ExitPressedOnce = false; };

        SetInitialSettings();
    }
    private void SetInitialSettings()
    {
        FullscreenButton.ButtonPressed = (bool)ConfigController.Config.GetValue("Settings", "Fullscreen", false);
        SoundButton.ButtonPressed = (bool)ConfigController.Config.GetValue("Settings", "Sound", true);
        Engine.MaxFps = Mathf.Clamp((int)ConfigController.Config.GetValue("Settings", "MaxFps", 60), 24, 480);
        DisplayServer.WindowSetVsyncMode((bool)ConfigController.Config.GetValue("Settings", "Vsync", true) ? DisplayServer.VSyncMode.Enabled : DisplayServer.VSyncMode.Disabled);
    }
    private void OnFullscreenButtonToggled(bool Fullscreen)
    {
        DisplayServer.WindowSetMode(Fullscreen ? DisplayServer.WindowMode.Fullscreen : DisplayServer.WindowMode.Windowed);
        ConfigController.SaveSetting("Settings", "Fullscreen", Fullscreen);
    }
    private void OnSoundButtonToggled(bool EnableSound)
    {
        Godot.AudioServer.SetBusMute(0, !EnableSound);
        ConfigController.SaveSetting("Settings", "Sound", EnableSound);
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
