using Godot;
using System;
using System.Security.Cryptography.X509Certificates;

public partial class Lightworks : Node3D
{
    public static Lightworks Instance;
    [Export] public MeshInstance3D eyeMesh;
    [Export] public Godot.Collections.Array<Light3D> spotlights = new();
    [Export] public Godot.Collections.Array<float> lightIntensities = new();
    public static void InitializationSequence()
    {
        for (int i = 0; i < Instance.spotlights.Count; i++)
        {
            int index = i;
            Tween tween = Instance.CreateTween();
            tween.TweenProperty(Instance.spotlights[index], "light_energy", Instance.lightIntensities[index] / 16f, 0.5f);
        }
        Tween eyeTween = Instance.CreateTween();
        eyeTween.TweenInterval(1 + (Instance.spotlights.Count * 0.5));
        eyeTween.TweenCallback(Callable.From(() => { Instance.eyeMesh.Transparency = 0; }));
    }


    public override void _Ready()
    {
        base._Ready();
        if (Instance == null) Instance = this;
        else if (Instance != this) { QueueFree(); return; }

        

    }
}
