using Godot;
using System;
using System.Collections.Generic;

[GlobalClass]
public partial class DevilHand : Hand
{
    public static DevilHand Instance;


    public override bool AllowPlay
    {
        get => base.AllowPlay;
        set
        {
            base.AllowPlay = value;
            if (value)
            {
                RestoreHand();
                GD.Print("aloooooo");
                GD.Print($"fora - card: {cards.Count} tile {Tabletop.table[3][3]}");
                this.PlayCard(cards[0], Tabletop.table[3][3]);
            }
        }
    }

    public override void PlayCard(Card card, TabletopTile tile)
    {
        GD.Print("caraio");
        GD.Print($"card: {card} tile {tile}");
        card.effect.ApplyEffects(tile);
        Discard(card);
        TurnState.OnInterruptPlayPhase?.Invoke();
    }


    public override void _Ready()
    {
        base._Ready();
        if (Instance == null) Instance = this;
        else if (Instance != this) { QueueFree(); return; }

    }


}
