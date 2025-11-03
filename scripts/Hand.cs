using Godot;
using System;
using System.Collections.Generic;


public abstract partial class Hand : Node3D
{



    public static Hand Instance { get; set; }
    


    

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
            if (value){ RestoreHand(); }
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


    public static void RestoreHand()
    {
        while (Instance.cards.Count < Instance.maxCards) DrawNewCard();
    }

    public static void DrawNewCard()
    {
        Instance.cards.Add(Instance.deck.GetTopCard());
    }

    public static void Discard(Card card)
    {
        Instance.cards.Remove(card);
        Instance.deck.PlaceAtBottom(card.effect);
    }

    public static void Replace(Card card) { Discard(card); DrawNewCard(); }



    public abstract void PlayCardWrapped(Card card, TabletopTile tile);

    public static void PlayCard(Card card, TabletopTile tile)
    {
        Instance.PlayCardWrapped(card, tile);
    }

    public override void _Ready()
    {
        base._Ready();

        if (Instance == null) Instance = this;
        else if (Instance != this) { QueueFree(); return; }


        RestoreHand();
        SpreadCards();
    }


}
