using Godot;
using System;

[GlobalClass]
public partial class TurnState : Node3D
{
    public static bool isPlayerTurn;

    public static Tween turnStateTween;

    public static TurnState Instance;

    public static void StartTurn()
    {
        turnStateTween = Instance.CreateTween();
        turnStateTween.TweenInterval(1);
        turnStateTween.TweenCallback(Callable.From(async () =>
        {
            Tabletop.MoveCreatures();
            await Instance.ToSignal(turnStateTween, Tween.SignalName.Finished);
        }));
    }

    public static void PlayPhase()
    { 
        turnStateTween = Instance.CreateTween();
        turnStateTween.TweenInterval(10);
        turnStateTween.TweenCallback(Callable.From(EndTurn));
    }


    public static void EndTurn()
    {
        
        isPlayerTurn = !isPlayerTurn;

        StartTurn();
    }

    public override void _Ready()
    {
        base._Ready();
        if (Instance == null) Instance = this;
        else if (Instance != this) { QueueFree(); return; }

    }
}
