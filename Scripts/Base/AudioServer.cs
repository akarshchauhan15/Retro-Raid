using Godot;
using Godot.Collections;

public partial class AudioServer : Node
{
    static Dictionary<string, AudioStreamPlayer> Streams = new();

    public override void _Ready()
    {
        foreach (AudioStreamPlayer Child in GetChildren())
            Streams.Add(Child.Name, Child);
    }
    public static void Play(string Audio) => Streams[Audio].Play();
}
