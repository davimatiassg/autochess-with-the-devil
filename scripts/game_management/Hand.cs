using Godot;
using System;
using System.Collections.Generic;


public abstract partial class Hand : Node3D
{


    [Export]
    public int maxCards = 3;

    [Export]
    public float cardSize = 1f;
    [Export]
    public float cardSeparation = 0.5f;

    [Export]
    protected Godot.Collections.Array<CardEffect> cards = new();

    [Export]
    public Deck deck;




    public bool _AllowPlay = false;
    public virtual bool AllowPlay
    {
        get => _AllowPlay;
        set
        {
            _AllowPlay = value;
        }
    }










   

    public abstract void DrawNewCard();



    public override void _Ready()
    {
        base._Ready();

    }
    


}
