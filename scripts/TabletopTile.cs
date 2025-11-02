using Godot;
using System;
using System.Collections.Generic;
using System.Drawing;

[GlobalClass]
public partial class TabletopTile : Area3D
{
    public Vector2I tilePosition;

    public List<PlacedObject> objectsInThisTile = new();

    public bool IsTileValid(CardEffect effect)
    {
        //STUB: tem que verificar a situação da carta
        return true;
    }

    public Creature containsCreature()
    {
        foreach (var obj in objectsInThisTile) if (obj is Creature creature) return creature; 
        return null;
    }
}