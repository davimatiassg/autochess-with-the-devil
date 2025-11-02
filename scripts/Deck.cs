using Godot;
using System;
using System.Collections.Generic;

[GlobalClass]
public partial class Deck : Node3D
{
    [Export]
    public Godot.Collections.Array<CardEffect> cards = new();

    public List<CardEffect> cardList = new();


    public override void _Ready()
    {
        base._Ready();
        foreach (var card in cards)
        {
            cardList.Add(card);
        }
    }

    public void PlaceAtBottom(CardEffect card)
    {
        cardList.Add(card);
    }

    public CardEffect GetTopCard()
    {
        var ret = cardList[0];
        cardList.RemoveAt(0);
        return ret;
    }
    

}
