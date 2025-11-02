using Godot;
using System;

[GlobalClass]
public partial class Creature : Sprite3D
{
    [Export]
    int currentHp;
    [Export]
    int currentDamage;

    [Export]
    public CreatureData data;

    public Creature(CreatureData data)
    {
        this.data = data;
        this.currentHp = data.hp;
        this.currentDamage = data.damage;
    }
}
