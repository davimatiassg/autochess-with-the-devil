using Godot;
using System;
using System.Threading.Tasks;

public partial class GameManager : Node
{

    public static GameManager Instance;

    public static Action<bool> OnStopGame;

    private static readonly PackedScene MenuScene = GD.Load<PackedScene>("res://scenes/MainMenu.tscn");

    public static int[] scores = new int[2];
    public static int PlayerScore
    {
        get => scores[0];
        set
        {
            scores[0] = value;
            AudioPlayer.Play("strumHigh");
        }
    }

    public static int EnemyScore
    {
        get => scores[1];
        set
        {
            scores[1] = value;
            AudioPlayer.Play("strumLow");
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
        if (scores[0] > 2)
        {
            await GameEnd(2);
        }
        if (scores[1] > 2)
        {
            await GameEnd(1);
        }
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

    public static async void PreGame()
    {
        await DialogMessenger.SpawnDialog((Godot.Collections.Array)GameDialogs.DialogData["Pre_Menu"]);
        GameMenu();
    }

    public static void PreGameWrapper()
    {
        PreGame();
    }

    public static async void GameStart()
    {
        AudioPlayer.Play("music", true);
        await DialogMessenger.SpawnDialog((Godot.Collections.Array)GameDialogs.DialogData["Game_Start"]);
    }

    public static async Task GameEnd(int i)
    {
        await DialogMessenger.SpawnDialog((Godot.Collections.Array)GameDialogs.DialogData[$"End_{i}"]);
        
    }

    public static void GameMenu()
    {

        MainMenu menu = (MainMenu)MenuScene.Instantiate();
        DialogMessenger.Instance.AddChild(menu);
        menu.playGame += GameStart;
        menu.exitGame += () => GameEnd(1);
    }

    public static void Credits()
    {

        //TODO: Show proper credits.
        Instance.GetTree().Quit();
    }

    public override void _Ready()
    {
        base._Ready();
        if (Instance == null) Instance = this;
        else if (Instance != this) { QueueFree(); return; }

        CallDeferred(MethodName.PreGameWrapper);

    }

}
