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
        this.CurrentHP = (int)(data.minValues -1 + (rng.NextInt64() % (1 + data.maxValues - data.minValues)));
        this.CurrentDamage = (int)(data.minValues - 1 + (rng.NextInt64() % (1 + data.maxValues - data.minValues)));
        this.SpriteFrames = data.sprite;

        if (!isPlayerObject) this.Modulate = Colors.DarkRed;

        this.Play();

        this.Scale = Vector3.One * 0.3f;
        this.Billboard = BaseMaterial3D.BillboardModeEnum.Enabled;
    }




    public void AttackCreature(Creature creature)
    {

        var currentPos = Position;
        animationTween = CreateTween();
        animationTween.TweenProperty(this, "position", creature.Position, 0.3).SetTrans(Tween.TransitionType.Expo);
        animationTween.TweenCallback(Callable.From(() => creature.CurrentHP -= CurrentDamage));
        animationTween.TweenCallback(Callable.From(() => data.AttackEffect(creature)));
        animationTween.TweenProperty(this, "position", currentPos, 0.5);
        animationTween.TweenCallback(Callable.From(() => creature.DefendFromCreature(this))); 
    }

    public void DefendFromCreature(Creature attacker)
    {
        if (CurrentHP > 0) data.SurviveEffect(attacker);
        else
        {
            data.DeathEffect(attacker);
            Remove();
        }
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
