using Godot;
using System;
using System.Collections.Generic;


public abstract partial class Hand : Node3D
{


    [Export]
    public int maxCards = 3;

    [Export]
    public float cardSize = 1f;
    public float cardSeparation = 0.5f;
    List<Card> cards = new();

    [Export]
    public Deck deck;




    public bool _AllowPlay = false;
    public virtual bool AllowPlay
    {
        get => _AllowPlay;
        set
        {
            _AllowPlay = value;
        }
    }







    public void SpreadCards()
    {
        Vector3 startPos = new Vector3(-(maxCards * cardSize + (maxCards - 1) * cardSeparation) / 2, 0, 0);
        float i = 0;

        foreach (var card in cards)
        {
            if (card.GetParent() == null) AddChild(card);
            card.Position = startPos + i * (cardSize + cardSeparation) * Vector3.Right;
            i++;
        }
    }


    public void RestoreHand()
    {
        while (cards.Count < maxCards) DrawNewCard();
    }

    public void DrawNewCard()
    {
        cards.Add(deck.GetTopCard());
    }

    public void Discard(Card card)
    {
        cards.Remove(card);
        deck.PlaceAtBottom(card.effect);
    }

    public void Replace(Card card) { Discard(card); DrawNewCard(); }



    public abstract void PlayCard(Card card, TabletopTile tile);

    public override void _Ready()
    {
        base._Ready();

    }


}
