using Godot;
using System;

[GlobalClass]
public partial class Card : Area3D
{
    
    public CardEffect effect;

    public void Select()
    {
        EmitSignal("OnSelectCard", this, TurnState.isPlayerTurn);
    }

    public Card() { }

    public Card(CardEffect effect)
    {
        this.effect = effect;
        //TODO! Make the card visuals appear!
    }

    public bool TryPlace(TabletopTile tile)
    {
        if (tile.IsTileValid(this.effect)) //Verificar se a posição é válida
        {
            effect.ApplyEffects(tile.tilePosition);
            return true;
        }
        return false;
    }


    public override void _Ready()
    {
        base._Ready();
        InputCaptureOnDrag = true;
        InputRayPickable = true;
    }

    private Vector3 defaultPos;

    Tween bobTween;
    public void OnMouseEntered()
    {
        bobTween = CreateTween();
        bobTween.TweenProperty(this, "position", defaultPos + Vector3.Up/5, 0.1);
    }

    public void OnMouseExited()
    {
        bobTween = CreateTween();
        bobTween.TweenProperty(this, "position", defaultPos ,0.1);
    }

    
}
