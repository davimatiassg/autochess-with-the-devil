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

        creature.GlobalPosition = tile.GlobalPosition;
        creature.Tile = tile;

        var nextTile = Tabletop.GetNextTile(tile, Vector2I.Right);
        var predicate = (PlacedObject p) => p is Creature c && c.isPlayerObject != creature.isPlayerObject;
        if (nextTile != null && nextTile.objectsInThisTile.FirstOrDefault(predicate) != null) creature.Move(nextTile);
        else nextTile = Tabletop.GetNextTile(tile, Vector2I.Left);
        if (nextTile != null && nextTile.objectsInThisTile.FirstOrDefault(predicate) != null) creature.Move(nextTile);
        else nextTile = Tabletop.GetNextTile(tile, Vector2I.Up);
        if (nextTile != null && nextTile.objectsInThisTile.FirstOrDefault(predicate) != null) creature.Move(nextTile);
        else nextTile = Tabletop.GetNextTile(tile, Vector2I.Down);
        if (nextTile != null && nextTile.objectsInThisTile.FirstOrDefault(predicate) != null) creature.Move(nextTile);

        
    }
}
