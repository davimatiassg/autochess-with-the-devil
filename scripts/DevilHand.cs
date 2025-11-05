using Godot;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

[GlobalClass]
public partial class DevilHand : Hand
{
    public static DevilHand Instance;


    public void StartTurn()
    {
        
    }

    public void PlayCard(CardEffect card, TabletopTile tile)
    {

        card.ApplyEffects(tile);
        cards.Remove(card);
        deck.PlaceAtBottom(card);
        TurnState.InterruptPlayPhase();
    }


    public override void _Ready()
    {
        base._Ready();
        if (Instance == null) Instance = this;
        else if (Instance != this) { QueueFree(); return; }
    
        TurnState.OnStartPlayPhase += () =>
        {
            
            //STUB: TODO! make proper ai for the devil
            if (TurnState.isPlayerTurn) return;

            DrawNewCard();

            Tween tween = CreateTween();
            tween.TweenInterval(1);
            tween.TweenCallback(Callable.From( () => this.PlayCard(cards[0], Tabletop.table[1][4])));
            
        };  

    }

    public override void DrawNewCard()
    {
        var newCard = deck.GetTopCard();
        cards.Add(newCard);
    }
}
