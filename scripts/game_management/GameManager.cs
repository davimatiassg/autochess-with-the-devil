using Godot;
using System;
using System.Threading.Tasks;

public partial class GameManager : Node
{

    public static GameManager Instance;

    public static Action<bool> OnStopGame;

    public static int[] scores = new int[2];
    public static int PlayerScore
    {
        get => scores[0];
        set
        {
            scores[0] = value;
            //TODO! Update ui to show player victories
            if (value > 2) { }
        }
    }

    public static int EnemyScore
    {
        get => scores[1];
        set
        {
            scores[1] = value;
            //TODO! Update ui to show enemy victories
            if (value > 2) { }
        }
    }

    public static void RoundEnd(bool playerWon)
    {
        TurnState.IsRoundRunning = false;

        OnStopGame?.Invoke(playerWon);
    }

    public static void RoundStart()
    {
        TurnState.IsRoundRunning = true;
        _ = TurnState.LoopTurns();
    }

    public override void _Ready()
    {
        base._Ready();
        if (Instance == null) Instance = this;
        else if (Instance != this) { QueueFree(); return; }

        CallDeferred(MethodName.RoundStart);

    }

}
