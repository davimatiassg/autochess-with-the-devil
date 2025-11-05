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

    public static async Task LoopTurn()
    {

        while (true)
        {

            
            OnStartTurn?.Invoke();

            if(!isPlayerTurn)Instance.playerCamera.SlideLookAt(DevilHand.Instance.Position, 0.5);

            await Tabletop.MoveCreatures();


            Instance.playerHand.AllowPlay = isPlayerTurn;
            Instance.devilHand.AllowPlay = !isPlayerTurn;


            OnStartPlayPhase();


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

    public override void _Ready()
    {
        base._Ready();
        if (Instance == null) Instance = this;
        else if (Instance != this) { QueueFree(); return; }

        isPlayerTurn = true;    
    
        _ = LoopTurn();
    }
}
