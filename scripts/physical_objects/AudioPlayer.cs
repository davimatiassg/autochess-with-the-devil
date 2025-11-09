using Godot;
using System;

public partial class AudioPlayer : Node
{

    public static AudioPlayer Instance;

    public static Random rng;

    [Export]
    public Godot.Collections.Dictionary sounds = new();

    public static void Play(string sound, bool loop = false, float pitch = 1, float volume = 0)
    {
        AudioStreamOggVorbis soundStream = AudioStreamOggVorbis.LoadFromFile(Instance.sounds[sound].AsString());
        var streamPlayer = new AudioStreamPlayer();
        Instance.AddChild(streamPlayer);
        streamPlayer.Stream = soundStream;
        soundStream.Loop = loop;
        streamPlayer.PitchScale = pitch;
        streamPlayer.VolumeDb = volume;
        streamPlayer.Play();
        streamPlayer.Finished += () => streamPlayer.QueueFree();
    }

    public static void PlayRandomPitch(string sound, bool loop = false, float pitch = 1, float variation = 0.1f, float volume = 0)
    {
        float newPitch = pitch + (float)(((rng.NextDouble() * 2) - 1) * variation);
        Play(sound, loop, newPitch, volume);
    }

    public override void _Ready()
    {
        base._Ready();
        if (Instance == null) Instance = this;
        else if (Instance != this) QueueFree();
        rng = new Random();
        Play("wind chimes", true, 0.5f, -2f);
    }
}
