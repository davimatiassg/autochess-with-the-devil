using Godot;
using System;

[GlobalClass]
public partial class Card : Area3D
{
    [Export]
    public CardEffect effect;

    [Export]
    public Sprite3D sprite;

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
