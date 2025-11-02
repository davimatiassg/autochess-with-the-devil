using Godot;
using System;

[GlobalClass]
public partial class TurnState : Node3D
{
    [Export]
    public PlayerHand playerHand;
    public DevilHand devilHand;

    public static bool isPlayerTurn;

    public static Tween turnStateTween;

    public static TurnState Instance;


    [Signal]
    public delegate void OnTurnEndEventHandler();


    public static void StartTurn()
    {
        turnStateTween = Instance.CreateTween();
        turnStateTween.TweenCallback(Callable.From(async () =>
        {
            Tabletop.MoveCreatures();
            await Instance.ToSignal(turnStateTween, Tween.SignalName.Finished);
            PlayPhase();
        }));
    }

    public static void PlayPhase()
    { 
        turnStateTween = Instance.CreateTween();
        turnStateTween.TweenInterval(15);
        turnStateTween.TweenCallback(Callable.From(EndCurrentTurn));
    }


    public static void EndCurrentTurn()
    {

        isPlayerTurn = !isPlayerTurn;
        Instance.playerHand.AllowPlay = isPlayerTurn;
        Instance.devilHand.AllowPlay = !isPlayerTurn;
        
        turnStateTween = Instance.CreateTween();
        turnStateTween.TweenInterval(1);
        turnStateTween.TweenCallback(Callable.From(StartTurn));
    }

    public override void _Ready()
    {
        base._Ready();
        if (Instance == null) Instance = this;
        else if (Instance != this) { QueueFree(); return; }


        OnTurnEnd += EndCurrentTurn;
    }
}
