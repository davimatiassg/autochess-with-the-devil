using Godot;
using System;

[GlobalClass]
public partial class Card : Node3D
{
    
    public CardEffect effect;

    [Export]
    public Area3D collision;

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

    public bool TryPlace(Vector2I position)
    {
        if (Tabletop.GetPlaceablePositions(effect).Contains(position))
        {
            effect.ApplyEffects(position);
            return true;
        }
        return false;
    }


    public override void _Ready()
    {
        base._Ready();
        collision.InputCaptureOnDrag = true;
        collision.InputRayPickable = true;
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
