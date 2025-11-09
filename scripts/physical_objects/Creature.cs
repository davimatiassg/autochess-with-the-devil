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

    public int CurrentHP
    {
        get => currentHp;
        set
        {
            currentHp = value;
            hpLabel.Text = $"{value}";
        }
    }
    public int CurrentDamage{
        get => currentDamage;
        set
        {
            currentDamage = value;
            atklabel.Text = $"{value}";
        }
    }

    [Export]
    public Label3D atklabel;
    [Export]
    public Label3D hpLabel;

    public override TabletopTile Tile
    {
        get => base.Tile;
        set
        {

            base.Tile = value;

            animationTween = CreateTween();
            animationTween.TweenProperty(this, "global_position", value.TileTop + Scale.Y / 2 * Vector3.Up, 0.3);

        }
    }

    [Export]
    public CreatureData data;

    public Tween animationTween;

    static Random rng = new Random();
    public void SetValues(CreatureData data)
    {
        this.data = data;
        this.CurrentHP = data.hp;
        this.CurrentDamage = data.damage;
        this.SpriteFrames = data.sprite;

        if (!isPlayerObject) this.Modulate = Colors.DarkRed;

        this.Play();

        this.Scale = Vector3.One * 0.3f;
        this.Billboard = BaseMaterial3D.BillboardModeEnum.Enabled;
    }




    public void AttackCreature(Creature creature, bool hasEffect = true)
    {

        var currentPos = Position;
        animationTween = CreateTween();
        animationTween.TweenProperty(this, "position", creature.Position, 0.3).SetTrans(Tween.TransitionType.Expo);
        animationTween.TweenCallback(Callable.From(() => creature.CurrentHP -= CurrentDamage));
        animationTween.TweenCallback(Callable.From(() => { if (hasEffect) data.AttackEffect(this, creature);  }));
        animationTween.TweenProperty(this, "position", currentPos, 0.5);
        animationTween.TweenCallback(Callable.From(() => creature.DefendFromCreature(this))); 
    }

    public void DefendFromCreature(Creature attacker, bool hasEffect = true)
    {
        if (CurrentHP > 0 && hasEffect) data.SurviveEffect(this, attacker);
        else
        {
            if(hasEffect) data.DeathEffect(this, attacker);
            Remove();
        }
    }

    public void Move(Vector2I direction, bool hasEffect = true) => Move(Tabletop.GetNextTile(Tile, direction), hasEffect);
    public virtual void Move(TabletopTile nextTile, bool hasEffect = true)
    {
        var possibleTarget = nextTile.containsCreature();
        if (possibleTarget != null)
        {
            if (possibleTarget.isPlayerObject != isPlayerObject)
            {
                AttackCreature(possibleTarget, hasEffect);

            }
            return;
        }
        animationTween = CreateTween();
        animationTween.TweenCallback(Callable.From(() =>
        {
            Tile.RemoveObject(this);
            nextTile.AddObject(this);
            Tile = nextTile;
            if(hasEffect) data.MoveEffect(this);
        }));
    }
    

}
