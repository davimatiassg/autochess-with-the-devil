using Godot;
using System;
using System.Collections.Generic;
using System.Drawing;

[GlobalClass]
public partial class Tabletop : Node3D
{
    public const int TABLETOP_WIDTH = 4;
    public const int TABLETOP_HEIGHT = 5;

    [Export]
    public Node3D tableRectBegin;
    [Export]
    public float tileSize = 0.1f;

    public static TabletopTile[][] table = new TabletopTile[TABLETOP_HEIGHT][];

    public static Tabletop Instance;

    public static Tween animationTween;






    [Signal]
    public delegate void OnPlaceCreatureEventHandler(CreatureData card, Vector2I position);



    public static List<Vector2I> GetPlaceablePositions(CardEffect effect)
    {

        var list = new List<Vector2I>();

        for (int y = 0; y < TABLETOP_HEIGHT; y++)
        {
            for (int x = 0; x < TABLETOP_WIDTH; x++)
            {
                list.Add(new Vector2I(x, y));
            }
        }

        return list;

        // if (effect is SpawnCreatureCardEffect)
        // {

        // }
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
        creature.position = position;
        
        
    }

    // TODO!
    // public void PlaceSpell(SpellCard card, Vector2I position)
    // {

    // }

    // public void PlaceTrap(TrapCard card, Vector2I position)
    // {

    // }

    public static TabletopTile GetNextTile(TabletopTile current, int direction)
    {
        int newY = current.tilePosition.Y + direction;

        if(newY >= TABLETOP_HEIGHT || newY < 0){ return null;  }

        return table[current.tilePosition.X][newY];
    }

    public static void MoveCreatures()
    {
        animationTween = Instance.CreateTween();
        if (TurnState.isPlayerTurn) Instance.MovePlayerCreatures();
        else Instance.MoveEnemyCreatures();
        animationTween.TweenInterval(0.5);
    }

    public void MovePlayerCreatures()
    {
        for (int y = 0; y < TABLETOP_HEIGHT; y++)
        { 
            for (int x = 0; x < TABLETOP_WIDTH; x++)
            {
                var tile = table[x][y];
                var creature = tile.containsCreature();
                if (creature != null)
                {
                    TryMoveCreature(creature, tile, -1);
                }
            }
        }
    }
    
    public void MoveEnemyCreatures()
    {
        for (int y = TABLETOP_HEIGHT-1; y >= 0; y--)
        { 
            for (int x = TABLETOP_WIDTH-1; x >= 0 ; x--)
            {
                var tile = table[x][y];
                var creature = tile.containsCreature();
                if (creature != null)
                {
                    TryMoveCreature(creature, tile, 1);
                }
            }
        }
    }

    public void TryMoveCreature(Creature creature, TabletopTile currentTile, int direction)
    {
        var nextTile = GetNextTile(currentTile, direction);
        

        if (nextTile == null)
        {
            EmitSignal("RoundOver", TurnState.isPlayerTurn);///TODO: Make this signal mean that the current player won the round.
            //TODO: Add death animation

            animationTween.Kill();
            return;
        }

        var nextTileCreature = nextTile.containsCreature();

        if (nextTileCreature == null)
        {
            animationTween.TweenProperty(creature, "position", new
            Vector3(nextTile.Position.X, creature.Position.Y, nextTile.Position.Z), 0.5);

            currentTile.objectsInThisTile.Remove(creature);
            nextTile.objectsInThisTile.Add(creature);

            creature.position = nextTile.tilePosition;

            creature.data.MoveEffect();
        }
        else if (nextTileCreature.isPlayerObject != TurnState.isPlayerTurn)
        {
            animationTween.TweenProperty(creature, "position",
                new Vector3(nextTile.Position.X, creature.Position.Y, nextTile.Position.Z), 0.2).SetTrans(Tween.TransitionType.Quad);
            animationTween.TweenProperty(creature, "position", new Vector3(currentTile.Position.X, creature.Position.Y, currentTile.Position.Z), 0.3);
            animationTween.TweenInterval(0.5);


            nextTileCreature.currentHp -= creature.currentDamage;

            creature.data.AttackEffect(nextTileCreature);

            if (nextTileCreature.currentHp > 0)
            {
                nextTileCreature.data.SurviveEffect(creature);
            }
            else
            {
                nextTileCreature.data.DeathEffect(creature);
                animationTween.TweenInterval(1);
                animationTween.TweenCallback(Callable.From(() => KillCreature(nextTileCreature, nextTile)));
            }
        }


        //TODO: if (objects[table[x][i]] is Trap) <-activate trap card->

    }

    public void KillCreature(Creature creature, TabletopTile tile)
    { 
        tile.objectsInThisTile.Remove(creature);
        creature.QueueFree();
    }

    public static Vector3 TranslatePositionToLocalSpace(int x, int y)
    {
        return Instance.tableRectBegin.Position + Instance.tileSize * new Vector3(x, y, 0);
    }


    public override void _Ready()
    {
        base._Ready();
        if (Instance == null) Instance = this;
        else if (Instance != this) {QueueFree(); return;}

        for (int x = 0; x < TABLETOP_WIDTH; x++)
        {
            table[x] = new TabletopTile[TABLETOP_HEIGHT];
            for (int y = 0; y < TABLETOP_HEIGHT; y++)
            {
                var tile = new TabletopTile { Scale = Vector3.One/this.Scale.X};

                var mesh = new MeshInstance3D()
                {
                    Mesh = new BoxMesh { Size = new Vector3(tileSize, 1, tileSize)  },
                };

                tile.AddChild(mesh);

                var collision = new CollisionShape3D();
                collision.Shape = new BoxShape3D { Size = new Vector3(tileSize, 1, tileSize) };

                tile.AddChild(collision);

                tile.tilePosition = new Vector2I(x, y);
                table[x][y] = tile;


                tile.Position = tableRectBegin.Position + (Vector3.Right * (x * tileSize)) + (Vector3.Forward * (y * tileSize ));
                

                AddChild(tile);
            }
        }

        OnPlaceCreature += PlaceCreature;
    }

}