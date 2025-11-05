using Godot;
using System;
using System.Collections.Generic;

[GlobalClass]
public partial class PlayerHand : Hand
{
    [Export]
    public Node3D rightHand;

    public static PlayerHand Instance;

    [Export] public double turnTimeLimit = 0.0;


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

        TurnState.InterruptPlayPhase();

        card.QueueFree();
    }
    
    public void SpreadCards()
    {
        Vector3 startPos = new Vector3(-(maxCards * cardSize + (maxCards - 1) * cardSeparation) / 2, 0, 0);
        float i = 0;

        foreach (var card in cards)
        {
            if (card.GetParent() == null) AddChild(card);
            card.Position = startPos + i * (cardSize + cardSeparation) * Vector3.Right;
            i++;
        }
    }


    public void StartPlayPhase()
    {
        RestoreHand();
        SpreadCards();



        if (turnTimeLimit > 0.0)
        {

            Tween loopPlayPhase = Instance.CreateTween();
            loopPlayPhase.TweenInterval(turnTimeLimit);
            loopPlayPhase.TweenCallback(Callable.From(TurnState.InterruptPlayPhase));
            TurnState.InterruptPlayPhase += () => { if (loopPlayPhase != null) loopPlayPhase.Kill(); };
        }
    }



    public override void _Ready()
    {
        base._Ready();
        if (Instance == null) Instance = this;
        else if (Instance != this) { QueueFree(); return; }

        TurnState.OnStartPlayPhase += () => { if (TurnState.isPlayerTurn) StartPlayPhase(); };  

    }

}
