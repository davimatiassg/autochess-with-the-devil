using Godot;
using System;

[GlobalClass]
public partial class CreatureData : Resource
{
    public int damage;
    public int hp;

    public virtual void SpawnEffect()
    { }
    public virtual void MoveEffect()
    { }
    public virtual void AttackEffect(PlacedObject target)
    { }
    public virtual void SurviveEffect(PlacedObject attacker)
    { }
    public virtual void DeathEffect(PlacedObject killer)
    { }
}
