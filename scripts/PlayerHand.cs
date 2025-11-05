using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

[GlobalClass]
public partial class PlayerHand : Hand
{
    [Export]
    public Node3D rightHand;

    [Export]
    public PackedScene CardPrefab;
    public static PlayerHand Instance;

    [Export] public double turnTimeLimit = 0.0;


    public static void PickCard(Card card)
    {
        Instance.RemoveChild(card);
        card.Position = Vector3.Zero;
        Instance.rightHand.AddChild(card);

    }

    public static void DropCard(Card card)
    {

        Instance.rightHand.RemoveChild(card);
        
        Instance.AddChild(card);
        Instance.SpreadCards();

    }

    public override void DrawNewCard()
    {
        var newCardEffect = deck.GetTopCard();

        
        cards.Add(newCardEffect);

        var newCard = (Card)CardPrefab.Instantiate();

        
        newCard.effect = newCardEffect;

        newCard.sprite.Texture = newCardEffect.portrait;


        AddChild(newCard);
    }

    public void PlayCard(Card card, TabletopTile tile)
    {

        card.effect.ApplyEffects(tile);

        var cardEffect = card.effect;
        

        cards.Remove(cardEffect);
        deck.PlaceAtBottom(cardEffect);

        card.effect = null;
        card.QueueFree();

        TurnState.InterruptPlayPhase();

        
    }
    
    public void SpreadCards()
    {
        Vector3 startPos = new Vector3(-(maxCards * cardSize + (maxCards - 1) * cardSeparation) / 2, 0, 0);
        float i = 0;

        foreach (Node3D card in GetChildren())
        {
            card.Position = startPos + i * (cardSize + cardSeparation) * Vector3.Right;
            i++;
        }
    }

    

    public void StartPlayPhase()
    {
        

        if (!TurnState.isPlayerTurn) return;



        while (cards.Count < maxCards)
        {
            DrawNewCard();
            
        } 
            

        SpreadCards();

        
        
        if (turnTimeLimit > 0.0)
        {
            Tween turnTimer = Instance.CreateTween();

            void InterruptTurn()
            {
                turnTimer.Kill();
                TurnState.InterruptPlayPhase -= InterruptTurn;
            }
            
            TurnState.InterruptPlayPhase += InterruptTurn;

            turnTimer.TweenInterval(turnTimeLimit);
            
            turnTimer.TweenCallback(Callable.From(TurnState.InterruptPlayPhase));

            
        }
    }



    public override void _Ready()
    {
        base._Ready();
        if (Instance == null) Instance = this;
        else if (Instance != this) { QueueFree(); return; }

        TurnState.OnStartPlayPhase += StartPlayPhase; 

    }

}
