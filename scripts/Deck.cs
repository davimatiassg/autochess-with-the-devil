using Godot;
using System;
using System.Collections.Generic;

[GlobalClass]
public partial class Deck : Node3D
{
    [Export]
    public PackedScene CardPrefab;

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

    public Card GetTopCard()
    {
        GD.Print(cards[0]);

        var topCardEffect = cards[0];
        cards.RemoveAt(0);

        var card = (Card)CardPrefab.Instantiate();
        card.effect = topCardEffect;
        card.sprite.Texture = card.effect.portrait;

        return card;
    }
    

}
