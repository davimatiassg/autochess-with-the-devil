using Godot;
using System;
using System.Collections.Generic;

[GlobalClass]
public partial class Hand : Node3D
{
    [Export]
    public int maxCards = 3;

    [Export]
    public int cardSize = 64;
    public int cardSeparation = 12;
    List<Card> cards;


    [Signal]
    public delegate void OnSelectCardEventHandler(Card card, bool showPlaceablePositions);



    public void SelectCard(Card card, bool showPlaceablePositions = false)
    {


        List<Vector2I> PlaceablePositions = Tabletop.GetPlaceablePositions(card.effect);

        //TODO: Spawn placeable positions vfx for the selected card;

    }

    public void DrawCard(CardEffect effect)
    {
        //TODO!
    }

    public void DiscardCard(Card card)
    {
        //TODO!
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


    public bool _AllowPlay = false;
    public bool AllowPlay
    {
        get => _AllowPlay;
        set
        {
            _AllowPlay = value;

        }
    }


    public override void _Ready()
    {
        base._Ready();
        OnSelectCard += SelectCard;
    }


}
