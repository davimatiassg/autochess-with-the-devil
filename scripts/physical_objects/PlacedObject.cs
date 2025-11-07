using Godot;
using System;

[GlobalClass]
public partial class PlacedObject : AnimatedSprite3D
{
    [Export]
    public virtual TabletopTile Tile { get; set; }

    [Export]
    public bool isPlayerObject;
}
