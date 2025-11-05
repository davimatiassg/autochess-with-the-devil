using Godot;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

[GlobalClass]
public partial class DevilHand : Hand
{
    public static DevilHand Instance;

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

    public void Act()
    {
        //STUB: TODO! make proper ai for the devil
        this.PlayCard(cards[0], Tabletop.table[1][4]);
    }

    /// <summary>
    /// Configures the method to act once the devil's turn has started
    /// </summary>
    public override void _Ready()
    {
        base._Ready();
        if (Instance == null) Instance = this;
        else if (Instance != this) { QueueFree(); return; }

        TurnState.OnStartPlayPhase += () =>
        {

            
            if (TurnState.isPlayerTurn) return;

            DrawNewCard();

            Tween tween = CreateTween();
            tween.TweenInterval(1);
            tween.TweenCallback(Callable.From(Act));

        };

    }

    /// <summary>
    /// Draws a new card from the deck.
    /// </summary>
    public override void DrawNewCard()
    {
        var newCard = deck.GetTopCard();
        cards.Add(newCard);
    }
}
