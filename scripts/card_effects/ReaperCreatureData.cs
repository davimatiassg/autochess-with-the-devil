using Godot;
using System;

public partial class ReaperCreatureData : CreatureData
{
    public override void SurviveEffect(Creature thisCreature, PlacedObject attacker)
    {
        base.SurviveEffect(thisCreature, attacker);
        thisCreature.CurrentHP += 2;
    }

    public override void AttackEffect(Creature thisCreature, PlacedObject attacker)
    {
        base.SurviveEffect(thisCreature, attacker);
        thisCreature.CurrentHP += 2;
    }
}
