using Godot;
using System;

public partial class AngelCreatureData : CreatureData
{

    public override void AttackEffect(Creature thisCreature, PlacedObject creature)
    {
        base.AttackEffect(thisCreature, creature);
        TabletopTile tile = Tabletop.GetNextTile(thisCreature.Tile, new Vector2I(0, TurnState.isPlayerTurn ? 1 : -1));

        if (tile.IsTileValid(new SpawnCreatureCardEffect()))
        { 
            thisCreature.Move(tile, false);
        }

        
    }
}
