using Godot;
using System;

public partial class GhostCreatureData : CreatureData
{
    public override void DeathEffect(Creature thisCreature, PlacedObject obj)
    {
        base.DeathEffect(thisCreature, obj);
        if (obj is Creature creature) creature.DefendFromCreature(thisCreature, false);        
    }
}
