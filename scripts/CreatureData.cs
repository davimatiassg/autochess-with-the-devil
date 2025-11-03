using Godot;
using System;

[GlobalClass]
public partial class CreatureData : Resource
{
    [Export]
    public int damage;

    [Export]
    public int hp;

    public Texture2D PlayTexture;

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
