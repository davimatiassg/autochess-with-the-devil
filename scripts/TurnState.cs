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

    public static TurnState Instance;

    public static Action OnStartTurn;

    public static Action OnStartPlayPhase;

    private static bool interrupt = false;
    public static Action InterruptPlayPhase = () => interrupt = true;

    public static Action OnEndTurn;



    /// <summary>
    /// The time limit for each turn's playing phase. Values equal or less than 0.1 make the turn infinite. 
    /// </summary>
    [Export] public double turnTime = 0;

    public static async Task LoopTurn()
    {
        InterruptPlayPhase += () => GD.Print("INTERRUPT!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
        while (true)
        {
            Instance.playerCamera.SlideLookAt(isPlayerTurn ? PlayerHand.Instance.Position : DevilHand.Instance.Position, 0.5);
            Instance.playerHand.AllowPlay = false;
            Instance.devilHand.AllowPlay = false;
            OnStartTurn?.Invoke();
            var x = isPlayerTurn ? "player" : "inimigo";
            GD.Print($"Startou {x}");
            await Tabletop.MoveCreatures();
            GD.Print("moveu");

            Instance.playerHand.AllowPlay = isPlayerTurn;
            Instance.devilHand.AllowPlay = !isPlayerTurn;
            OnStartPlayPhase?.Invoke();
            
            GD.Print($"Playphase fez {x}");
            await PlayPhase();
            GD.Print($"Terminou {x}");


            await EndCurrentTurn();
            OnEndTurn?.Invoke();


            isPlayerTurn = !isPlayerTurn;
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
