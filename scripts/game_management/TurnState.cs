using Godot;
using System;
using System.Linq;
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

    public static TurnState Instance;

    public static Action OnStartTurn;

    public static Action OnStartPlayPhase;

    private static bool interrupt = false;
    public static Action InterruptPlayPhase = () => interrupt = true;

    public static Action OnEndTurn;

    public static bool IsRoundRunning = false;

    public static async Task LoopTurns()
    {
        isPlayerTurn = true;
        while (true)
        {


            OnStartTurn?.Invoke();

            if (!isPlayerTurn) Instance.playerCamera.SlideLookAt(DevilHand.Instance.Position, 0.5);

            await Tabletop.MoveCreatures();

            if (!IsRoundRunning) break;


            Instance.playerHand.AllowPlay = isPlayerTurn;
            Instance.devilHand.AllowPlay = !isPlayerTurn;


            OnStartPlayPhase?.Invoke();


            await PlayPhase();



            await EndCurrentTurn();
            OnEndTurn?.Invoke();


            isPlayerTurn = !isPlayerTurn;

            Instance.playerHand.AllowPlay = false;
            Instance.devilHand.AllowPlay = false;


        }

    }




    private static async Task PlayPhase()
    {
        
        while (!interrupt) { await Task.Delay(100); }

        interrupt = false;
    }


    private static async Task EndCurrentTurn()
    {

        Instance.playerHand.AllowPlay = false;
        Instance.devilHand.AllowPlay = false;

        

        await Task.Delay(500);
    }

    public async override void _Ready()
    {
        base._Ready();
        if (Instance == null) Instance = this;
        else if (Instance != this) { QueueFree(); return; }

        await ToSignal(this, SignalName.Ready);

        GameManager.OnStopGame += (bool _) => IsRoundRunning = false;
        
    
    }
}
