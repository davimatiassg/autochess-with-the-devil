using Godot;
using System;

[GlobalClass]
public partial class Card : Node3D
{
    public CardEffect effect;

    public void Select()
    {
        EmitSignal("OnSelectCard", this, Tabletop.isPlayerTurn);
    }

    public void TryPlace(Vector2I position)
    {
        //TODO: Call signal to unselect a card
        if (Tabletop.GetPlaceablePositions(effect).Contains(position))
        {
            effect.ApplyEffects(position);

            //TODO: Call signal to discard a card
        }        
    }

    
}
