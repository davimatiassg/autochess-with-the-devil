using Godot;
using System;

[GlobalClass]
public partial class PlacedObject : AnimatedSprite3D
{
    [Export]
    public virtual TabletopTile Tile { get; set; }

    [Export]
    public bool isPlayerObject;


    public void Remove()
    {
        var deathTween = CreateTween();

        deathTween.TweenInterval(0.1);
        deathTween.TweenProperty(this, "modulate", Colors.Transparent, 0.5);
        deathTween.TweenCallback(Callable.From(() =>
            {
                Tile.RemoveObject(this);
                QueueFree();
            })
        );
    }
}
