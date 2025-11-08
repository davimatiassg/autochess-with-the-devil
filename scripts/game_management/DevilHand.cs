using Godot;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

[GlobalClass]
public partial class DevilHand : Hand
{
    public static DevilHand Instance;

    public static Godot.Collections.Array nextDialog = default;

    public static Random rng = new();

    private DevilAI AiInstance = new DevilAI();

    //AI States: Waiting Turn -> (Event OnStartPlayPhase) -> Thinking (Act) -> Calling (PlayCard) -> Waiting Turn, repeat
    private CardEffect _bestCardToPlay;
    private TabletopTile _bestTileToPlay;
    private int _bestMoveScore;

    private List<TabletopTile> _playableTilesList = new List<TabletopTile>();

    public void PlayCard(CardEffect card, TabletopTile tile)
    {
        card.ApplyEffects(tile);
        cards.Remove(card);
        deck.PlaceAtBottom(card);
        TurnState.InterruptPlayPhase();
    }

    /// <summary>
    /// Where the IA decides what is the best play to do
    /// </summary>
    public void Act()
    {
        //Use the AI instance and comunicate the List of cards, and playable tiles
        BestMove bestMove = AiInstance.FindBestMove(new List<CardEffect>(cards), _playableTilesList);

        if (bestMove != null)
        {
            PlayCard(bestMove.CardToPlay, bestMove.TileToPlayOn);
        }
        else
        {
            TurnState.InterruptPlayPhase();
        }
    }


    /// <summary>
    /// Basic Play Phase routine.
    /// </summary>
    public async void PlayPhase()
    {
        if (TurnState.isPlayerTurn) return;

        DrawNewCard();

        if (nextDialog != default)
        {
            await DialogMessenger.SpawnDialog(nextDialog);
            nextDialog = default;
        }


        Tween tween = CreateTween();
        tween.TweenInterval(1);
        tween.TweenCallback(Callable.From(Act));
        tween.TweenCallback(Callable.From(TurnState.InterruptPlayPhase));
    }

    /// <summary>
    /// Draws a new card from the deck.
    /// </summary>
    public override void DrawNewCard()
    {
        var newCard = deck.GetTopCard();
        cards.Add(newCard);
    }

    /// <summary>
    /// Configures the method to act once the devil's turn has started
    /// </summary>
    public override void _Ready()
    {
        base._Ready();
        if (Instance == null) Instance = this;
        else if (Instance != this) { QueueFree(); return; }

        //Lê fileira de trás do Demônio é Y = boardHeight - 1
        int backRowY = Tabletop.Instance.boardHeight - 1;

        for (int x = 0; x < Tabletop.Instance.boardWidth; x++)
        {
            //Acessa o tile no array estático do Tabletop já
            TabletopTile tile = Tabletop.table[x][backRowY];
            if (tile != null)
            {
                _playableTilesList.Add(tile);
            }
        }
        
        TurnState.OnStartPlayPhase += () =>
        {
            if (TurnState.isPlayerTurn)
            {
                return;
            }
            

            DrawNewCard();

            Tween tween = CreateTween();
            tween.TweenInterval(1);

            tween.TweenCallback(Callable.From(Act));
        };
    }
};