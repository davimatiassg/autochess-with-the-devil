using Godot;
using System;
using System.Linq;

[GlobalClass]
public partial class SpawnCreatureCardEffect : CardEffect
{
    private static readonly PackedScene CreatureScene = GD.Load<PackedScene>("res://scenes/Creature.tscn");
    [Export]
    public CreatureData creatureData;
    public override void ApplyEffects(TabletopTile tile)
    {

        //stub
        creatureData.PlayTexture = portrait;


        Creature creature = (Creature)CreatureScene.Instantiate();
        creature.isPlayerObject = TurnState.isPlayerTurn;

        creature.SetValues(creatureData);


        Tabletop.Instance.AddChild(creature);
        tile.objectsInThisTile.Add(creature);

        creature.Tile = tile;

        creature.GlobalPosition = tile.TileTop + creature.Scale.Y / 2 * Vector3.Up;
        FirstMove(creature);

    }

    public void FirstMove(Creature creature)
    {
        Vector2I direction = new Vector2I(0, creature.isPlayerObject ? 1 : -1);




        var hasEnemy = (PlacedObject p) => p is Creature c && c.isPlayerObject != creature.isPlayerObject;
        var hasAlly = (PlacedObject p) => p is Creature c && c.isPlayerObject == creature.isPlayerObject;


        var frontTile = Tabletop.GetNextTile(creature.Tile, direction);

        var canMoveForward = frontTile.objectsInThisTile.Count == 0 || frontTile.objectsInThisTile.FirstOrDefault(hasAlly) == default;




        direction = Vector2I.Right;
        var nextTile = Tabletop.GetNextTile(creature.Tile, direction);
        if (nextTile != null && nextTile.objectsInThisTile.FirstOrDefault(hasEnemy) != default) { creature.Move(nextTile); return; }

        direction = Vector2I.Left;
        nextTile = Tabletop.GetNextTile(creature.Tile, direction);
        if (nextTile != null && nextTile.objectsInThisTile.FirstOrDefault(hasEnemy) != default) { creature.Move(nextTile); return; }

        if (canMoveForward) { creature.Move(frontTile); return; }
        

        
    }
}
