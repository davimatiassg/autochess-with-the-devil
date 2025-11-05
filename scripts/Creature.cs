using Godot;
using System;
using System.Threading.Tasks;

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
            animationTween.TweenProperty(this, "global_position", value.TileTop + Scale.Y/2 * Vector3.Up, 0.3);
            
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




    public void AttackCreature(Creature creature)
    {

        var currentPos = Position;
        animationTween = CreateTween();
        animationTween.TweenProperty(this, "position", creature.Position, 0.3).SetTrans(Tween.TransitionType.Expo);
        animationTween.TweenCallback(Callable.From(() => creature.currentHp -= currentDamage));
        animationTween.TweenCallback(Callable.From(() => data.AttackEffect(creature)));
        animationTween.TweenProperty(this, "position", currentPos, 0.3);
        animationTween.TweenCallback(Callable.From(() => DefendFromCreature(creature)));        
    }

    public void DefendFromCreature(Creature attacker)
    {
        if (currentHp > 0) data.SurviveEffect(attacker);
        else data.DeathEffect(attacker);
    }


    public void Move(TabletopTile nextTile)
    {
        var possibleTarget = nextTile.containsCreature();
        if (possibleTarget != null)
        {
            if (possibleTarget.isPlayerObject != isPlayerObject)
            {
                AttackCreature(possibleTarget);

            }
            return;
        }
        animationTween = CreateTween();
        animationTween.TweenCallback(Callable.From(() =>
        {
            Tile.RemoveObject(this);
            nextTile.AddObject(this);
            Tile = nextTile;
            data.MoveEffect();
        }));
    }
    

    public void Die()
    {

    }
}
