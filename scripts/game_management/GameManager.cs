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

    public static async void RoundEnd(bool playerWon)
    {
        TurnState.IsRoundRunning = false;
        OnStopGame?.Invoke(playerWon);

        var tween = Instance.CreateTween();
        tween.TweenInterval(0.5);
        tween.TweenCallback(Callable.From(() => { Tabletop.Instance.AnimateBoardTransition(false, 0.5); }));

        if (playerWon) PlayerScore++;
        else EnemyScore++;

        await DialogMessenger.SpawnDialog((Godot.Collections.Array)GameDialogs.DialogData[playerWon ? "Win_" + PlayerScore : "Loss_" + EnemyScore]);

        //TODO! something, idk, show the images I guess;

        RoundStart(); 
        
    }

    public static void RoundStart()
    {
        TurnState.IsRoundRunning = true;
        _ = TurnState.LoopTurns();

        var tween = Instance.CreateTween();
        tween.TweenInterval(0.5);
        tween.TweenCallback(Callable.From(() => { Tabletop.Instance.AnimateBoardTransition(); }));
        
           

    }

    public override void _Ready()
    {
        base._Ready();
        if (Instance == null) Instance = this;
        else if (Instance != this) { QueueFree(); return; }

        CallDeferred(MethodName.RoundStart);

    }

}
