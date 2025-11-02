using Godot;
using System;

[GlobalClass]
public partial class PlacedObject : Sprite3D
{
    [Export]
    public Vector2I position;

    [Export]
    public bool isPlayerObject;
}
