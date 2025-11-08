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
                if (table[x][y].IsTileValid(effect)) list.Add(table[x][y]);
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
        animationTween = Instance.CreateTween();
        animationTween.TweenInterval(0.5);
        if (TurnState.isPlayerTurn) await Instance.MovePlayerCreatures();
        else await Instance.MoveEnemyCreatures();
        
    }

    public async Task MovePlayerCreatures()
    {
        
        for (int y = boardHeight-1; y >= 0; y--)
        { 
            for (int x = boardWidth-1; x >= 0 ; x--){
                var tile = table[x][y];
                var creature = tile.containsCreature();
                if (creature != null)
                {
                    if(creature.isPlayerObject) await TryMoveCreature(creature, tile, 1);
                }
            }
        }
    }
    
    public async Task MoveEnemyCreatures()
    {
        for (int y = 0; y < boardHeight; y++)
        { 
            for (int x = 0; x < boardWidth; x++)
            {
                var tile = table[x][y];
                var creature = tile.containsCreature();
                if (creature != null)
                {
                    if (!creature.isPlayerObject) await TryMoveCreature(creature, tile, -1);
                }
            }
        }
    }

    public async Task TryMoveCreature(Creature creature, TabletopTile currentTile, int direction)
    {
        var nextTile = GetNextTile(currentTile, direction);

            
        if (nextTile == null)
        {
            GameManager.RoundEnd(TurnState.isPlayerTurn);

            animationTween.Kill();
            return;
        }

        creature.Move(nextTile);
        
        while (creature.animationTween.IsRunning()) await Task.Delay(1000);

        //TODO: if (nextTile.ContainsTrap() != null) <-activate trap card->

    }


    public void SpawnBoard()
    {
        var tableOffset = new Vector3(tileSize * boardHeight, 0, tileSize * boardWidth) / 2;


        bool isBlack = true;

        table = new TabletopTile[boardWidth][];
        for (int x = 0; x < boardWidth; x++)
        {
            table[x] = new TabletopTile[boardHeight];
            if (boardHeight % 2 == 0) isBlack = !isBlack;
            for (int y = 0; y < boardHeight; y++)
            {
                var tile = (TabletopTile)tilePrefab.Instantiate();

                tile.tilePosition = new Vector2I(x, y);
                table[x][y] = tile;

                tile.Size = new Vector3(tileSize, 0.015f, tileSize);

                tableRectCenter.AddChild(tile);

                tile.Position = new Vector3((y + 0.5f) * tileSize, 10f, (x + 0.5f) * tileSize) - tableOffset;

                if (isBlack) tile.meshInstance.MaterialOverlay = blackMaterial;
                else tile.meshInstance.MaterialOverlay = whiteMaterial;

                isBlack = !isBlack;

            }
        }

    }

    public void AnimateBoardTransition(bool isDownwards = true, double timePerTile = 0.1)
    {
        var tableOffset = new Vector3(tileSize * boardHeight, 0, tileSize * boardWidth) / 2;
        float targetY = isDownwards ? 0 : 5f;

        Tween animationTween = CreateTween();
        animationTween.SetParallel();

        for (int y = 0; y < boardHeight; y++)
        {
            for (int x = 0; x < boardWidth; x++)
            {
                foreach (var obj in table[x][y].objectsInThisTile) obj.Remove();
                Vector3 pos = new Vector3((y + 0.5f) * tileSize, targetY, (x + 0.5f) * tileSize) - tableOffset;
                animationTween.TweenProperty(table[x][y], "position", pos, (x + y) * timePerTile + 0.2);
            }
        }
    }

    public override void _Ready()
    {
        base._Ready();

        if (Instance == null) Instance = this;
        else if (Instance != this) { QueueFree(); return; }

        SpawnBoard();
    }

}