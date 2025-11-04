using Godot;
using System;

[GlobalClass]
public partial class SpawnCreatureCardEffect : CardEffect
{
    [Export]
    public CreatureData creatureData;
    public override void ApplyEffects(TabletopTile tile)
    {
        
        //stub
        creatureData.PlayTexture = portrait;


        Creature creature = new Creature(creatureData);
        creature.isPlayerObject = TurnState.isPlayerTurn;

        Tabletop.Instance.AddChild(creature);
        tile.objectsInThisTile.Add(creature);

        creature.GlobalPosition = tile.GlobalPosition;
        creature.Tile = tile;
        
    }
}
