using Godot;
using System;
using System.Collections.Generic;

[GlobalClass]
public partial class Deck : Node3D
{

    [Export]
    public Godot.Collections.Array<CardEffect> cards = new();



    public override void _Ready()
    {
        base._Ready();
        Shuffle();
    }

    public void PlaceAtBottom(CardEffect card)
    {
        cards.Add(card);
    }

    public void Shuffle()
    {
        var rng = new RandomNumberGenerator();
        for (int i = cards.Count - 1; i > 0; i--)
        {
            int j = (int)rng.RandiRange(0, i);
            var temp = cards[i];
            cards[i] = cards[j];
            cards[j] = temp;
        }
    }

    public CardEffect GetTopCard()
    {

        var topCardEffect = cards[0];
        cards.RemoveAt(0);



        return topCardEffect;
    }

}
