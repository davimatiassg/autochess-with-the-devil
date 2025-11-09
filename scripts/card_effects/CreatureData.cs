using Godot;
using System;
using System.Security.Cryptography.X509Certificates;

[GlobalClass]
public partial class CreatureData : Resource
{
    [Export]
    public int damage;

    [Export]
    public int hp;

    [Export]
    public SpriteFrames sprite;

    public Texture2D PlayTexture;

    public virtual void SpawnEffect(Creature thisCreature)
    { }
    public virtual void MoveEffect(Creature thisCreature)
    { }
    public virtual void AttackEffect(Creature thisCreature, PlacedObject target)
    { }
    public virtual void SurviveEffect(Creature thisCreature, PlacedObject attacker)
    { }
    public virtual void DeathEffect(Creature thisCreature, PlacedObject killer)
    { }
}
