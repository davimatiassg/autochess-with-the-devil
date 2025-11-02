using Godot;
using System;

[GlobalClass]
public abstract partial class CardEffect : Resource
{
    [Export]
    public Texture2D portrait;
    [Export]
    public string description = "";
    public abstract void ApplyEffects(Vector2I position);

}
