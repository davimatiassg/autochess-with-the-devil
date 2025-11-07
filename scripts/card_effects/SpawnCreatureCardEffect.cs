using Godot;
using System;

[GlobalClass]
public partial class SpawnCreatureCardEffect : CardEffect
{
    private static readonly PackedScene CreatureScene = GD.Load<PackedScene>("res://scenes/Creature.tscn");
    [Export]
    public CreatureData creatureData;
    public override void ApplyEffects(TabletopTile tile)
    {
        
        //stub
        creatureData.PlayTexture = portrait;


        Creature creature = (Creature)CreatureScene.Instantiate();
        creature.isPlayerObject = TurnState.isPlayerTurn;
        
        creature.SetValues(creatureData);
        

        Tabletop.Instance.AddChild(creature);
        tile.objectsInThisTile.Add(creature);

        creature.GlobalPosition = tile.GlobalPosition;
        creature.Tile = tile;
        
    }
}
