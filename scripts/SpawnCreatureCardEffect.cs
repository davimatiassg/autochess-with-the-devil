using Godot;
using System;

[GlobalClass]
public partial class SpawnCreatureCardEffect : CardEffect
{
    [Export]
    public CreatureData creatureData;
    public override void ApplyEffects(TabletopTile tile)
    {
        creatureData.PlayTexture = portrait;
        Creature creature = new Creature(creatureData);
        Tabletop.Instance.AddChild(creature);
        creature.Tile = tile;

    }
}
