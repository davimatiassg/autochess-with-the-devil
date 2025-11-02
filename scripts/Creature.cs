using Godot;
using System;

[GlobalClass]
public partial class Creature : PlacedObject
{
    [Export]
    public int currentHp;
    [Export]
    public int currentDamage;

    [Export]
    public CreatureData data;

    public Creature(CreatureData data)
    {
        this.data = data;
        this.currentHp = data.hp;
        this.currentDamage = data.damage;
    }
}
