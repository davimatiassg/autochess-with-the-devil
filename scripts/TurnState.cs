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



    public static async Task StartTurn()
    {

        await Tabletop.MoveCreatures();
 
        await PlayPhaseAsync();

        _ = EndCurrentTurn();
    }

    public static async Task PlayPhaseAsync()
    {
        Instance.playerHand.AllowPlay = isPlayerTurn;
        Instance.devilHand.AllowPlay = !isPlayerTurn;
        turnStateTween = Instance.CreateTween();
        turnStateTween.TweenInterval(5);

        await Instance.ToSignal(turnStateTween, Tween.SignalName.Finished);
    }


    public static async Task EndCurrentTurn()
    {

        
        
        Instance.playerHand.AllowPlay = false;
        Instance.devilHand.AllowPlay = false;
        
        Vector3 lookDirection = Instance.playerCamera.Position - Instance.playerCamera.GlobalBasis.Z;

        turnStateTween = Instance.CreateTween();
        if (isPlayerTurn)
        {
            turnStateTween.TweenMethod(
                Callable.From((Vector3 target) => Instance.playerCamera.LookAt(target)),
                lookDirection,
                Instance.devilHand.Position,
                0.5)
                .SetTrans(Tween.TransitionType.Bounce);
        }
        else
        {
            turnStateTween.TweenMethod(Callable.From((Vector3 target) => Instance.playerCamera.LookAt(target)),
                lookDirection,
                Instance.playerHand.Position,
                0.8)
                .SetTrans(Tween.TransitionType.Circ);
        }
        turnStateTween.TweenCallback(Callable.From(() => isPlayerTurn = !isPlayerTurn));

        await Instance.ToSignal(turnStateTween, Tween.SignalName.Finished);
        _ = StartTurn();
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
