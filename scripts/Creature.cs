using Godot;
using System;

[GlobalClass]
public partial class Creature : PlacedObject
{
    [Export]
    public int currentHp;
    [Export]
    public int currentDamage;

    public override TabletopTile Tile
    {
        get => base.Tile;
        set
        {
            base.Tile = value;
            animationTween = CreateTween();
            animationTween.TweenProperty(this, "global_position", Tile.TileTop + Scale.Y/2 * Vector3.Up, 0.3);
            animationTween.TweenInterval(0.1);
            
        }
    }

    [Export]
    public CreatureData data;

    public Tween animationTween;

    public Creature(CreatureData data)
    {
        this.data = data;
        this.currentHp = data.hp;
        this.currentDamage = data.damage;
        this.Texture = data.PlayTexture;

        this.Scale = Vector3.One * 0.3f;
        this.Billboard = BaseMaterial3D.BillboardModeEnum.Enabled;
    }

    
    private async void AttackedResponse(Creature creature)
    {
        if (creature.currentHp > 0) creature.data.SurviveEffect(creature);
        else creature.data.DeathEffect(creature);

        await ToSignal(creature.animationTween, Tween.SignalName.Finished);
    }

    public void AttackCreature(Creature creature)
    {

        var currentPos = Position;
        animationTween = CreateTween();
        animationTween.TweenProperty(this, "position", creature.Position, 0.3).SetTrans(Tween.TransitionType.Expo);
        animationTween.TweenCallback(Callable.From(() => creature.currentHp -= currentDamage ));
        animationTween.TweenCallback(Callable.From(() => data.AttackEffect(creature)));
        animationTween.TweenProperty(this, "position", currentPos, 0.3);

        animationTween.TweenCallback(Callable.From(() => { AttackedResponse(creature); }));
        animationTween.TweenInterval(0.2);
    }


    public void Die()
    { 

    }
}
