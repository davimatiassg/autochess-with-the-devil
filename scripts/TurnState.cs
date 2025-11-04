using Godot;
using System;

[GlobalClass]
public partial class TurnState : Node3D
{
    [Export]
    public PlayerHand playerHand;
    [Export]
    public PlayerCamera playerCamera;

    [Export]
    public DevilHand devilHand;

    public static bool isPlayerTurn;

    public static Tween turnStateTween;

    public static TurnState Instance;



    [Signal]
    public delegate void OnTurnEndEventHandler();


    public static void StartTurn()
    {

        Action awaitForMove = async () =>
        {
            Tabletop.MoveCreatures();
            await Instance.ToSignal(Tabletop.animationTween, Tween.SignalName.Finished);
            PlayPhase();
        };
        turnStateTween.Stop();
        turnStateTween.TweenCallback(Callable.From(() => awaitForMove));
    }

    public static void PlayPhase()
    { 
        turnStateTween.TweenInterval(5);
        turnStateTween.TweenCallback(Callable.From(EndCurrentTurn));
    }


    public static void EndCurrentTurn()
    {

        isPlayerTurn = !isPlayerTurn;
        Instance.playerHand.AllowPlay = isPlayerTurn;
        Instance.devilHand.AllowPlay = !isPlayerTurn;
        
        Vector3 basisZ = Instance.playerCamera.Transform.Basis.Z;

        turnStateTween = Instance.CreateTween();
        turnStateTween.TweenInterval(1);
        if (!isPlayerTurn)
        {
            
            turnStateTween.TweenMethod(Callable.From(
                (Vector3 target) => Instance.playerCamera.LookAt(target)), basisZ, Instance.devilHand.Position, 0.2);
        }
        else
        {
            turnStateTween.TweenMethod(Callable.From(
                (Vector3 target) => Instance.playerCamera.LookAt(target)), basisZ, Instance.playerHand.Position, 0.5);
        }
        turnStateTween.TweenCallback(Callable.From(StartTurn));
    }

    public override void _Ready()
    {
        base._Ready();
        if (Instance == null) Instance = this;
        else if (Instance != this) { QueueFree(); return; }

        turnStateTween = Instance.CreateTween();
        turnStateTween.TweenInterval(1.0);
        turnStateTween.TweenCallback(Callable.From(StartTurn));
    }
}
