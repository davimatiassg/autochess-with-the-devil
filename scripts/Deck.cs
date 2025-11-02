using Godot;
using System;
using System.Collections.Generic;

[GlobalClass]
public partial class Deck : Node3D
{
    [Export]
    public Godot.Collections.Array<CardEffect> cards = new();

}
