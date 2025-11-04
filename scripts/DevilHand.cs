using Godot;
using System;
using System.Collections.Generic;

[GlobalClass]
public partial class DevilHand : Hand
{
    public static DevilHand Instance;

    public override void PlayCard(Card card, TabletopTile tile)
    {
        card.effect.ApplyEffects(tile);
        Discard(card);
    }

    public override void _Ready()
    {
        base._Ready();
        if (Instance == null) Instance = this;
        else if (Instance != this) { QueueFree(); return; }

    }
}
