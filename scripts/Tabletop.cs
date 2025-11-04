using Godot;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

[GlobalClass]
public partial class Tabletop : Node3D
{
    [Export]
    public int boardWidth = 4;
    [Export]
    public int boardHeight = 5;
    [Export]
    public float tileSize = 0.3f;

    [Export]
    public Node3D tableRectCenter;
    
    [Export]
    public PackedScene tilePrefab;

    [Export]
    Material blackMaterial;

    [Export]
    Material whiteMaterial;
    public static TabletopTile[][] table;

    public static Tabletop Instance;

    public static Tween animationTween;



    public static List<TabletopTile> GetPlaceablePositions(CardEffect effect)
    {

        var list = new List<TabletopTile>();

        for (int y = 0; y < Instance.boardHeight; y++)
        {
            for (int x = 0; x < Instance.boardWidth; x++)
            {
                if(table[x][y].IsTileValid(effect)) list.Add(table[x][y]);
            }
        }

        return list;
    }




    public static TabletopTile GetNextTile(TabletopTile current, int direction)
    {
        int newY = current.tilePosition.Y + direction;

        if(newY >= Instance.boardHeight || newY < 0){ return null;  }

        return table[current.tilePosition.X][newY];
    }

    public static async Task MoveCreatures()
    {
        GD.Print("Moving creatures");
        animationTween = Instance.CreateTween();
        animationTween.TweenInterval(0.5);
        if (TurnState.isPlayerTurn) await Instance.MovePlayerCreatures();
        else await Instance.MoveEnemyCreatures();
        
    }

    public async Task MovePlayerCreatures()
    {
        for (int y = 0; y < boardHeight; y++)
        { 
            for (int x = 0; x < boardWidth; x++)
            {
                var tile = table[x][y];
                var creature = tile.containsCreature();
                GD.Print($"inspecting tile {x}, {y}");
                if (creature != null)
                {
                    GD.Print($"moving creature on tile {x}, {y}");
                    await TryMoveCreatureAsync(creature, tile, -1);
                }
            }
        }
    }
    
    public async Task MoveEnemyCreatures()
    {
        for (int y = boardHeight-1; y >= 0; y--)
        { 
            for (int x = boardWidth-1; x >= 0 ; x--)
            {
                var tile = table[x][y];
                var creature = tile.containsCreature();
                if (creature != null)
                {
                    await TryMoveCreatureAsync(creature, tile, 1);
                }
            }
        }
    }

    public async Task TryMoveCreatureAsync(Creature creature, TabletopTile currentTile, int direction)
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

        if (nextTileCreature == null) //Move
        {
            animationTween.TweenCallback(Callable.From(() =>
            {
                creature.Tile = nextTile;
                currentTile.RemoveObject(creature);
                nextTile.AddObject(creature);
                creature.data.MoveEffect();
            }));
            animationTween.TweenInterval(1);

            
        }
        else if (nextTileCreature.isPlayerObject != TurnState.isPlayerTurn) //Attack
        {
            animationTween.TweenCallback(Callable.From(() =>
            {
                creature.AttackCreature(nextTileCreature);
            }));
            animationTween.TweenInterval(1);
        }

        
        await ToSignal(creature.animationTween, Tween.SignalName.Finished);

        //TODO: if (objects[table[x][i]] is Trap) <-activate trap card->

    }

    public void KillCreature(Creature creature, TabletopTile tile)
    { 
        tile.RemoveObject(creature);
        creature.QueueFree();
    }


    public override void _Ready()
    {
        base._Ready();

        if (Instance == null) Instance = this;
        else if (Instance != this) { QueueFree(); return; }

        var tableOffset = new Vector3(tileSize * boardHeight, 0, tileSize * boardWidth)/2;


        bool isBlack = true;

        table = new TabletopTile[boardWidth][];
        for (int x = 0; x < boardWidth; x++)
        {
            table[x] = new TabletopTile[boardHeight];
            for (int y = 0; y < boardHeight; y++)
            {
                var tile = (TabletopTile)tilePrefab.Instantiate();

                tile.tilePosition = new Vector2I(x, y);
                table[x][y] = tile;

                tile.Size = new Vector3(tileSize, 0.015f, tileSize);

                tableRectCenter.AddChild(tile);

                tile.Position = new Vector3((y+0.5f) * tileSize, 0, (x+0.5f) * tileSize) - tableOffset;

                if (isBlack) tile.meshInstance.MaterialOverlay = blackMaterial;
                else tile.meshInstance.MaterialOverlay = whiteMaterial;

                isBlack = !isBlack;

            }
        }
    }

}