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
        GD.Print($"restoring card {card.GetType()}");
        cards.Add(card);
    }

    public Card GetTopCard()
    {

        var topCardEffect = cards[0];
        cards.RemoveAt(0);

        var card = (Card)CardPrefab.Instantiate();
        card.effect = topCardEffect;
        card.sprite.Texture = card.effect.portrait;

        return card;
    }
    

}
