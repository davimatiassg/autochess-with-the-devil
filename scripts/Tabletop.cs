using Godot;
using System;
using System.Collections.Generic;
using System.Drawing;

[GlobalClass]
public partial class Tabletop : Node3D
{
    public const int TABLETOP_SIZE = 3;
    public static int[][] table = new int[TABLETOP_SIZE][];

    public static Tabletop Instance;

    public static Tween animationTween;

    public List<PlacedObject> objects = new();

    [Export]
    public Node3D tableRectBegin;

    [Export]
    public float tableSlotRectSize;


    

    [Signal]
    public delegate void OnPlaceCreatureEventHandler(CreatureData card, Vector2I position);


    public static List<Vector2I> GetPlaceablePositions(CardEffect effect)
    {
        if (effect is SpawnCreatureCardEffect)
        {
            var list = TurnState.isPlayerTurn ?
            new List<Vector2I> {
                new Vector2I(2, 0),
                new Vector2I(2, 1),
                new Vector2I(2, 2)
            }
            : new List<Vector2I> {
                new Vector2I(0, 0),
                new Vector2I(0, 1),
                new Vector2I(0, 2)
            };

            //TODO: Check whenever there are unities or traps in the chosen positions.

            return list;
        }
        // else if (card is SpellCard)
        // {

        // }
        // else if(card is TrapCard)
        // {

        // }


        return new List<Vector2I>();
    }
    



    public void PlaceCreature(CreatureData card, Vector2I position)
    {
        Creature creature = new Creature(card);

    }

    // TODO!
    // public void PlaceSpell(SpellCard card, Vector2I position)
    // {

    // }

    // public void PlaceTrap(TrapCard card, Vector2I position)
    // {

    // }


    public static void MoveCreatures()
    {
        animationTween = Instance.CreateTween();
        if (Instance.objects.Count < 1) return;
        if (TurnState.isPlayerTurn) Instance.MovePlayerCreatures();
        else Instance.MoveEnemyCreatures();
        animationTween.TweenInterval(0.5);
    }

    public void MovePlayerCreatures()
    {
        for (int y = 0; y < TABLETOP_SIZE; y++)
        { 
            for (int x = 0; x < TABLETOP_SIZE; x++)
            {
                var obj = objects[table[y][x]];
                if (obj is Creature creature && creature.isPlayerObject)
                {
                    TryMoveCreature(x,y, creature, -1);
                }
            }
        }
    }
    
    public void MoveEnemyCreatures()
    {
        for (int y = TABLETOP_SIZE-1; y >= 0; y--)
        { 
            for (int x = TABLETOP_SIZE-1; x >= 0 ; x--)
            {
                var obj = objects[table[y][x]];
                if (obj is Creature creature && !creature.isPlayerObject)
                {
                    TryMoveCreature(x,y, creature, 1);
                }
            }
        }
    }

    public void TryMoveCreature(int x, int y, Creature creature, int direction)
    {
        
        

        if (y + direction > TABLETOP_SIZE || y + direction < 0)
        {
            EmitSignal("RoundOver", TurnState.isPlayerTurn);///TODO: Make this signal mean that the current player won the round.
            //TODO: Add death animation

            animationTween.Kill();
            return;
        }

        if (table[y + direction][x] == -1)
        {
            animationTween.TweenProperty(creature, ":Position", TranslatePositionToLocalSpace(x, y + direction), 0.5);

            int tmp = table[y][x];
            table[y][x] = -1;
            y += direction;
            table[y][x] = tmp;

            creature.position = new Vector2I(x, y);

            creature.data.MoveEffect();
        }
        else if (objects[table[y + direction][x]] is Creature enemyCreature && enemyCreature.isPlayerObject != TurnState.isPlayerTurn)
        {
            animationTween.TweenProperty(creature, ":Position", TranslatePositionToLocalSpace(x, y + direction), 0.2).SetTrans(Tween.TransitionType.Quad);
            animationTween.TweenProperty(creature, ":Position", TranslatePositionToLocalSpace(x, y), 0.3);
            animationTween.TweenInterval(0.5);


            enemyCreature.currentHp -= creature.currentDamage;

            creature.data.AttackEffect(enemyCreature);

            if (enemyCreature.currentHp > 0)
            {
                enemyCreature.data.SurviveEffect(creature);
            }
            else
            {
                enemyCreature.data.DeathEffect(creature);
                animationTween.TweenInterval(1);
                animationTween.TweenCallback(Callable.From( () => KillCreature(enemyCreature) ) );
            }
        }


        //TODO: if (objects[table[x][i]] is Trap) <-activate trap card->

    }

    public void KillCreature(Creature creature)
    { 
        objects.Remove(creature);
        table[creature.position.Y][creature.position.X] = -1;
        creature.QueueFree();
    }

    public static Vector3 TranslatePositionToLocalSpace(int x, int y)
    {
        return Instance.tableRectBegin.Position + Instance.tableSlotRectSize * new Vector3(x, y, 0);
    }


    public override void _Ready()
    {
        base._Ready();
        if (Instance == null) Instance = this;
        else if (Instance != this) {QueueFree(); return;}

        for (int i = 0; i < TABLETOP_SIZE; i++) table[i] = new int[TABLETOP_SIZE];

        OnPlaceCreature += PlaceCreature;
    }

}