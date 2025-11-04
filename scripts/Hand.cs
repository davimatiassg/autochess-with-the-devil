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
    protected List<Card> cards = new();

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
