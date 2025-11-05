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
    }

    public void PlaceAtBottom(CardEffect card)
    {
        cards.Add(card);
    }

    public CardEffect GetTopCard()
    {

        var topCardEffect = cards[0];
        cards.RemoveAt(0);



        return topCardEffect;
    }
    

}
