using Godot;
using System;

public partial class Hask : Node
{

    public override void _Input(InputEvent @event)
    {
        base._Input(@event);

        if (@event.IsActionPressed("ui_accept"))
        {
            GameManager.RoundEnd(true);
        }
    }
}
