using Godot;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

[GlobalClass]
public partial class DevilHand : Hand
{
    public static DevilHand Instance;

    public static Godot.Collections.Dictionary nextDialog = default;

    //Variáveis pra guardar a melhore decisão com base no scoring
    //Estados da IA: Esperando Turno -> (Evento OnStartPlayPhase) -> Pensando (Act) -> Executando (PlayCard) -> Esperando Turno, dai cicla infinito
    private CardEffect _bestCardToPlay;
    private TabletopTile _bestTileToPlay;
    private int _bestMoveScore;

    //Lista dos tiles disponíveis pra jogar
    [Export]
    private Godot.Collections.Array<TabletopTile> _initialPlayableTiles = new();
    private List<TabletopTile> _playableTilesList = new List<TabletopTile>();

    /// <summary>
    /// Plays a card on a tile.
    /// </summary>
    /// <param name="card">The card</param>
    /// <param name="tile">The tile</param>
    public void PlayCard(CardEffect card, TabletopTile tile)
    {
        card.ApplyEffects(tile);
        cards.Remove(card);
        deck.PlaceAtBottom(card);
        TurnState.InterruptPlayPhase();
    }

    //Onde a IA age, e executa os estados
    public void Act()
    {
        // Estado 1: Inicia a Decisão
        // Reseta a cada turno
        _bestCardToPlay = null;
        _bestTileToPlay = null;
        _bestMoveScore = int.MinValue;

        //Estado 2: pensando na melhor jogada
        foreach (var card in cards)
        {
            foreach (var tile in _playableTilesList)
            {
                // FSM, cada jogada vira um estado
                //A EvaluateMove decide o melhor estado
                int currentMoveScore = EvaluateMove(card, tile);

                GD.Print($"Robozão tá pensando o seguinte pai: Serasi eu jogo '{card.ResourceName}' em '{tile.Name}', consigo um score massa de: {currentMoveScore}");

                if (currentMoveScore > _bestMoveScore)
                {
                    _bestMoveScore = currentMoveScore;
                    _bestCardToPlay = card;
                    _bestTileToPlay = tile;
                }
            }

        }

        // Estado 3: Joga
        if (_bestCardToPlay != null && _bestMoveScore > 0)
        {
            GD.Print($"Mofi, decidi que: Vo jogar '{_bestCardToPlay.ResourceName}' em '{_bestTileToPlay.Name}', consigo um score massa de: {_bestMoveScore}");
            PlayCard(_bestCardToPlay, _bestTileToPlay);
        }  else
        {
            //Estado 4: Fallback, se nenhuma jogada boa for encontrada
            GD.Print("O Robo tá achando que: Não tem nenhuma jogada que preste. Tentando fallback agr, é o que sobra pros betinha.");
            if (TryPlayAnyCardFallback())
            {
                return; //Sucesso no fallback
            }

            //Estado 5
            //Ficar ocioso e passar o turno se precisar
            GD.Print("Reiosse total, nada pra fazer");
            this.PlayCard(cards[0], Tabletop.table[1][4]); //Usando só pra testar se n queboru nada, ela ainda não tá pegando a _bestCard, ent tá caindo aqui nessa parte do loop
            //TurnState.InterruptPlayPhase();
        }
        //this.PlayCard(cards[0], Tabletop.table[1][4]);
    }
    
    /// <summary>
    ///  Aq é onde a FSM calcula os melhores valores pra decidir a prioridade de carta a se jogar
    /// </summary>
    /// <param name="card"></param>
    /// <param name="tile"></param>
    /// <returns>Score numerico, qnt maior melhor</returns>
    private int EvaluateMove(CardEffect card, TabletopTile tile)
    {
        //TODO verificar os status entre a carta que foi jogada por último e as da sua mão
        int score = 0;

        score = 1;

        return score;
    }

    private bool TryPlayAnyCardFallback()
    {
        if (cards.Count > 0 && _playableTilesList.Count > 0)
        {
            GD.Print("Fallback truta, jogando qq uma aq");
            PlayCard(cards[0], _playableTilesList[0]);
            return true;
        }
        return false;

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

        //Converte os tiles exportados pra uma lista
        foreach (var tile in _initialPlayableTiles)
        {
            _playableTilesList.Add(tile);
        }

        TurnState.OnStartPlayPhase += () =>
        {
            if (TurnState.isPlayerTurn) return;

            DrawNewCard();

            Tween tween = CreateTween();
            tween.TweenInterval(1);
            
            //Tween pra chamar o Act()
            tween.TweenCallback(Callable.From(Act));
        };
    }
}

