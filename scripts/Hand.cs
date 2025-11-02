using Godot;
using System;
using System.Collections.Generic;


public partial class Hand : Node3D
{
    [Export]
    public int maxCards = 3;

    [Export]
    public int cardSize = 64;
    public int cardSeparation = 12;
    List<Card> cards;

    public Deck deck;




    public bool _AllowPlay = false;
    public bool AllowPlay
    {
        get => _AllowPlay;
        set
        {
            _AllowPlay = value;
            if (value){ RestoreHand(); }
        }
    }







    public void SpreadCards()
    {
        Vector3 startPos = new Vector3(-(maxCards * cardSize + (maxCards - 1) * cardSeparation) / 2, 0, 0);
        float i = 0;

        foreach (var card in cards)
        {
            card.Position = startPos + i * (cardSize + cardSeparation) * Vector3.Right;
        }
    }


    public void RestoreHand()
    {
        while (cards.Count < maxCards) DrawNewCard();

        
    }

    public void DrawNewCard()
    {
        cards.Add(new Card(deck.GetTopCard()));
    }

    public void Discard(Card card)
    {
        cards.Remove(card);
        deck.PlaceAtBottom(card.effect);
    }

    public void Replace(Card card) { Discard(card); DrawNewCard(); }



    public virtual bool PlayCard(Card card, TabletopTile tile)
    {
        //TODO: Dispawn placeable positions vfx for the selected card;

        bool placed = card.TryPlace(tile);

        if (placed)
        {
            Discard(card);
            EmitSignal("OnTurnEnd");
        }

        return placed;
    }  

    public override void _Ready()
    {
        base._Ready();
        RestoreHand();
        SpreadCards();
    }


}
