using Godot;
using System;
using System.Collections.Generic;

[GlobalClass]
public partial class PlayerHand : Hand
{
    [Export]
    public Node3D rightHand;

    public static PlayerHand Instance;


    public override bool AllowPlay
    {
        get => base.AllowPlay;
        set
        {
            base.AllowPlay = value;
            if (!value)
            {
                //DropCard();
            }
        }
    }


    public static void PickCard(Card card)
    {
        Instance.RemoveChild(card);
        card.Position = Vector3.Zero;
        Instance.rightHand.AddChild(card);

        //TODO: Spawn placeable positions guide for the selected card;

    }

    public static void DropCard(Card card)
    {

        Instance.rightHand.RemoveChild(card);
        
        Instance.AddChild(card);
        Instance.SpreadCards();
        //TODO: Dispawn placeable positions guide for the selected card;

    }

    public override void PlayCard(Card card, TabletopTile tile)
    {
        //TODO: Dispawn placeable positions vfx for the selected card;

        card.effect.ApplyEffects(tile);

        Discard(card);
        
        Instance.rightHand.RemoveChild(card);
    }


    public override void _Ready()
    {
        base._Ready();
                if (Instance == null) Instance = this;
        else if (Instance != this) { QueueFree(); return; }

        RestoreHand();
        SpreadCards();
    }

}
