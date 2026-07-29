using Godot;

public partial class ConfigController : Node
{
    public static ConfigFile Config = new ConfigFile();

    public static string Path = "res://settings.ini";

    public override void _Ready()
    {
        
        if (!FileAccess.FileExists(Path))
        {
            ResetSettings();
            GetTree().Root.GetNode("Main/HUD").AddChild(GD.Load<PackedScene>("res://Scenes/Hud/InitialPage.tscn").Instantiate<InitialPage>());
        }
        else
            Config.Load(Path);
    }
    public static void ResetSettings()
    {
        Config.SetValue("Settings", "Sound", true);
        Config.SetValue("Settings", "Fullscreen", false);
        Config.SetValue("Settings", "MaxFps", 60);
        Config.SetValue("Settings", "Vsync", true);

        Config.Save(Path);
        AppendComment();
    }
    public static void SaveSetting(string Section, string Key, Variant Value)
    {
        Config.SetValue(Section, Key, Value);
        Config.Save(Path);
        AppendComment();
    }
    private static void AppendComment()
    {
        string Text = FileAccess.GetFileAsString(Path);
        using var File = FileAccess.Open(Path, FileAccess.ModeFlags.Write);

        string Comment = """ 


            # These settings can be changed by modifying their values.
            # Delete this file to reset settings to default.
            # MaxFps are clamped between 24 and 480. Values not lying in this range will be overridden.
            """;
        File.StoreString(Text + Comment);
    }

}
