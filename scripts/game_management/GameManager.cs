using Godot;
using System;

public partial class GameManager : Node
{

    public static GameManager Instance;

    public static Action<bool> StopGame;


    public override void _Ready()
    {
        base._Ready();
        if (Instance == null) Instance = this;
        else if (Instance != this) QueueFree();
    }

}
