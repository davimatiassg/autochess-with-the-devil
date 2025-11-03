using Godot;
using System;
using System.Collections.Generic;

[GlobalClass]
public partial class DevilHand : Hand
{
    public override void PlayCardWrapped(Card card, TabletopTile tile)
    { 
        card.effect.ApplyEffects(tile);
        Discard(card);
    } 
}
