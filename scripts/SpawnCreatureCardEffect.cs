using Godot;
using System;

[GlobalClass]
public partial class SpawnCreatureCardEffect : CardEffect
{
    [Export]
    public CreatureData creatureData;
    public override void ApplyEffects(Vector2I position)
    {
        EmitSignal("OnPlaceCreature", creatureData, position);
    }
}
