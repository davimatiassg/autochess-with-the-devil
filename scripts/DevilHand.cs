using Godot;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        }
    }

    public void StartTurn()
    {
        
    }

    public override void PlayCard(Card card, TabletopTile tile)
    {

        card.effect.ApplyEffects(tile);
        Discard(card);
        TurnState.InterruptPlayPhase();
    }


    public override void _Ready()
    {
        base._Ready();
        if (Instance == null) Instance = this;
        else if (Instance != this) { QueueFree(); return; }

        TurnState.OnStartPlayPhase += () =>
        {
            //STUB: make proper ai for the devil
            if (TurnState.isPlayerTurn) return;
            RestoreHand();
            Tween tween = CreateTween();
            tween.TweenInterval(1);
            tween.TweenCallback(Callable.From( () => this.PlayCard(cards[0], Tabletop.table[1][4])));
            
        };  

    }


}
