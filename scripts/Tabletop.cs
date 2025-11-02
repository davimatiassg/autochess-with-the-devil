using Godot;
using System;
using System.Collections.Generic;

[GlobalClass]
public partial class Tabletop : Node3D
{
    public static int[][] Table = new int[3][];

    public static bool isPlayerTurn = true;


    [Signal]
    public delegate void OnPlaceCreatureEventHandler(CreatureData card, Vector2I position);

    [Signal]
    public delegate void OnSelectCardEventHandler(Card card, bool showPlaceablePositions);


    public override void _Ready()
    {
        base._Ready();

        OnSelectCard += SelectCard;
        OnPlaceCreature += PlaceCreature;

    }

    public void PlaceCreature(CreatureData card, Vector2I position)
    {
        Creature creature = new Creature(card);

    }

    // TODO!
    // public void PlaceSpell(SpellCard card, Vector2I position)
    // {

    // }

    // public void PlaceTrap(TrapCard card, Vector2I position)
    // {

    // }


    public void SelectCard(Card card, bool showPlaceablePositions = false)
    {


        List<Vector2I> PlaceablePositions = GetPlaceablePositions(card.effect);

        //TODO: Spawn placeable positions vfx for the selected card;

    }

    public static List<Vector2I> GetPlaceablePositions(CardEffect effect)
    {
        if (effect is SpawnCreatureCardEffect)
        {
            return isPlayerTurn ?
            new List<Vector2I> {
                new Vector2I(0, 2),
                new Vector2I(1, 2),
                new Vector2I(2, 2)
            }
            : new List<Vector2I> {
                new Vector2I(0, 0),
                new Vector2I(1, 0),
                new Vector2I(2, 0)
            };
        }
        // else if (card is SpellCard)
        // {

        // }
        // else if(card is TrapCard)
        // {

        // }


        return new List<Vector2I>();
    }

}