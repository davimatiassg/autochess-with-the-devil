using Godot;
using System;
using System.Collections.Generic;

[GlobalClass]
public partial class PlayerHand : Hand
{
    public void SelectCard(Card card, bool showPlaceablePositions = false)
    {


        List<Vector2I> PlaceablePositions = Tabletop.GetPlaceablePositions(card.effect);

        //TODO: Spawn placeable positions guide for the selected card;

    }
    
}
