using Godot;

public partial class InitialPage : Control
{
    Button NextButton;
    Button PreviousButton;
    Button SkipButton;
    Label Counter;
    Timer SkipHoldTimer;
    ColorRect ProgressBar;

    float SkipTime = 0.6f;
    float MaxSize;
    int CurrentPage = 1;
    int TotalPages;

    Tween tween;

    public override void _Ready()
    {
        NextButton = GetNode<Button>("NextButton");
        PreviousButton = GetNode<Button>("PreviousButton");
        SkipButton = GetNode<Button>("SkipButton");
        Counter = GetNode<Label>("Counter");
        SkipHoldTimer = GetNode<Timer>("SkipHoldTimer");
        ProgressBar = GetNode<ColorRect>("SkipButton/Progress");

        TotalPages = GetNode<Control>("Pages").GetChildCount();

        NextButton.Pressed += () => ChangePage(true);
        PreviousButton.Pressed += () => ChangePage(false);
        SkipButton.ButtonDown += SkipButtonDown;
        SkipButton.ButtonUp += SkipButtonUp;
        SkipHoldTimer.Timeout += Destroy;

        SkipHoldTimer.WaitTime = SkipTime;

        Counter.Text = CurrentPage.ToString() + "/" + TotalPages.ToString();
        PreviousButton.Hide();
        MaxSize = SkipButton.Size.X;
    }
    private void ChangePage(bool MoveForward)
    {
        GetNode<Control>("Pages/" + CurrentPage.ToString()).Hide();

        CurrentPage += MoveForward ? 1 : -1;
        CurrentPage = Mathf.Clamp(CurrentPage, 1, TotalPages + 1);

        if (CurrentPage == TotalPages + 1) { Destroy(); return; }

        GetNode<Control>("Pages/" + CurrentPage.ToString()).Show();

        Counter.Text = CurrentPage.ToString() + "/" + TotalPages.ToString();
        PreviousButton.Visible = CurrentPage != 1;
    }
    private void Destroy()
    {
        PreviousButton.MouseFilter = MouseFilterEnum.Ignore;
        NextButton.MouseFilter = MouseFilterEnum.Ignore;

        SkipButton.ButtonDown -= SkipButtonDown;
        SkipButton.MouseFilter = MouseFilterEnum.Ignore;
        
        if (tween != null) tween.Kill();
        tween = CreateTween();

        tween.TweenProperty(this, "modulate:a", 0f, 0.4).SetTrans(Tween.TransitionType.Quad);
        tween.TweenCallback(Callable.From(() => QueueFree()));
    }
    private void SkipButtonDown()
    {
        SkipHoldTimer.Start();
        
        if (tween != null) tween.Kill();
        tween = CreateTween();
        tween.TweenProperty(ProgressBar, "size:x", MaxSize, SkipTime).SetTrans(Tween.TransitionType.Quad);
    }
    private void SkipButtonUp()
    {
        SkipHoldTimer.Stop();

        if (tween != null) tween.Kill();
        tween = CreateTween();
        tween.TweenCallback(Callable.From(() => SkipButton.Disabled = true));
        tween.TweenProperty(ProgressBar, "size:x", 0, 0.2f + Mathf.Lerp(0, 0.3f, ProgressBar.Size.X/MaxSize)).SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Quad);
        tween.SetParallel(false);
        tween.TweenCallback(Callable.From(() => SkipButton.Disabled = false));
    }
}
