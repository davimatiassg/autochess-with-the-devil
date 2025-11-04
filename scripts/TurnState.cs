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

    public static Action OnStartTurn;

    public static Action OnStartPlayPhase;

    private static bool interrupt = false;
    public static Action OnInterruptPlayPhase = () => interrupt = true;

    public static Action OnEndTurn;



    /// <summary>
    /// The time limit for each turn's playing phase. Values equal or less than 0.1 make the turn infinite. 
    /// </summary>
    [Export] public double turnTime = 0;

    public static async Task StartTurn()
    {
        OnStartTurn?.Invoke();
        await Tabletop.MoveCreatures();
        await PlayPhase();
        _ = EndCurrentTurn();
    }




    private static async Task PlayPhase()
    {
        Instance.playerHand.AllowPlay = isPlayerTurn;
        Instance.devilHand.AllowPlay = !isPlayerTurn;

        OnStartPlayPhase?.Invoke();

        turnStateTween = Instance.CreateTween();
        turnStateTween.TweenInterval(0.1);

        if (Instance.turnTime > 0.0)
        {
            turnStateTween.TweenInterval(Instance.turnTime);
            turnStateTween.TweenCallback(Callable.From(() => GD.Print("timeEnd")));
            turnStateTween.TweenCallback(Callable.From(() => interrupt = true));

        }
        while (!interrupt)
        {
            await Task.Delay(100);
        }
        turnStateTween.Stop();
        
    }


    private static async Task EndCurrentTurn()
    {

        OnEndTurn?.Invoke();
        
        Instance.playerHand.AllowPlay = false;
        Instance.devilHand.AllowPlay = false;
        
        
        interrupt = false;
        
        turnStateTween = Instance.CreateTween();

        Vector3 lookDirection = Instance.playerCamera.Position - Instance.playerCamera.GlobalBasis.Z;

        //TODO: this should go to the camera's script. 
        // Code to make the player look to the game
        if (isPlayerTurn)
        {
            turnStateTween.TweenMethod(
                Callable.From((Vector3 target) => Instance.playerCamera.LookAt(target)),
                lookDirection,
                Instance.devilHand.Position,
                0.5)
                .SetTrans(Tween.TransitionType.Circ);
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
