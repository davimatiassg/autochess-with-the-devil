using Godot;
using System;

[GlobalClass]
public abstract partial class CardEffect : Resource
{
    [Export]
    public Texture2D portrait;
    [Export]
    public string description = "";

    public double animationDuration = 0.2;
    public abstract void ApplyEffects(TabletopTile tile);

}
