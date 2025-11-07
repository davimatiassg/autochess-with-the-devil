using Godot;
using System;

[GlobalClass]
public partial class CreatureData : Resource
{
    [Export]
    public int maxValues;

    [Export]
    public int minValues;

    [Export]
    public SpriteFrames sprite;

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
