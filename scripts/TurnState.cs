using Godot;
using System;
using System.Threading.Tasks;

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


    public static async Task StartTurn()
    {
        await Task.Delay(800);
        await Tabletop.MoveCreatures();
        PlayPhase();
    }

    public static void PlayPhase()
    {
        GD.Print("Play Phase");
        Instance.playerHand.AllowPlay = isPlayerTurn;
        Instance.devilHand.AllowPlay = !isPlayerTurn;
        turnStateTween = Instance.CreateTween();
        turnStateTween.TweenInterval(5);
        turnStateTween.TweenCallback(Callable.From(EndCurrentTurn));
    }


    public static void EndCurrentTurn()
    {

        
        
        Instance.playerHand.AllowPlay = false;
        Instance.devilHand.AllowPlay = false;
        
        Vector3 basisZ = Instance.playerCamera.GlobalBasis.Z;

        turnStateTween = Instance.CreateTween();
        turnStateTween.TweenInterval(1);
        if (isPlayerTurn)
        {
            GD.Print("Ending Player's turn");
            turnStateTween.TweenMethod(Callable.From(
                (Vector3 target) => Instance.playerCamera.LookAt(target)), basisZ, Instance.devilHand.Position, 0.2);
        }
        else
        {
            turnStateTween.TweenMethod(Callable.From(
                (Vector3 target) => Instance.playerCamera.LookAt(target)), basisZ, Instance.playerHand.Position, 0.5);
        }
        turnStateTween.TweenCallback(Callable.From(() => isPlayerTurn = !isPlayerTurn));
        turnStateTween.TweenCallback(Callable.From(StartTurn));
    }

    public override void _Ready()
    {
        base._Ready();
        if (Instance == null) Instance = this;
        else if (Instance != this) { QueueFree(); return; }

        isPlayerTurn = true;
        _ = StartTurn();
    }
}
