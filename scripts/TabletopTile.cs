using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

[GlobalClass]
public partial class TabletopTile : Area3D
{
    public List<PlacedObject> objectsInThisTile = new();

    [Export]
    public MeshInstance3D meshInstance;
    [Export]
    public CollisionShape3D collision;

    public Vector3 TileTop { get => this.GlobalPosition + Vector3.Up * (this.Scale.Y * Size.Y); }


    
    public Vector2I tilePosition;

    Vector3 _Size;
    
    public Vector3 Size
    {
        get => _Size;
        set
        {
            _Size = value;
            var mesh = meshInstance.Mesh as BoxMesh;
            Debug.Assert(mesh != null);
            mesh.Size = value;
            var boxShape = collision.Shape as BoxShape3D;
            Debug.Assert(boxShape != null);
            boxShape.Size = value;
        }
    }

    public bool IsTileValid(CardEffect effect)
    {
        //STUB: tem que verificar a situação da carta
        if (containsCreature() != null) return false;

        if (effect is SpawnCreatureCardEffect)
        {
            if (TurnState.isPlayerTurn) {
                if (tilePosition.Y > 0) { return false; } }
            else if (tilePosition.Y < Tabletop.Instance.boardHeight - 1) return false;
        }

        return true;
    }

    public Creature containsCreature()
    {
        foreach (var obj in objectsInThisTile) if (obj is Creature creature) return creature;
        return null;
    }


    public void AddObject(PlacedObject placedObject)
    {
        objectsInThisTile.Add(placedObject);
        placedObject.Tile = this;

    }
    public void RemoveObject(PlacedObject placedObject)
    {
        objectsInThisTile.Remove(placedObject);
        placedObject.Tile = null;
        
    }

    public override void _Ready()
    {
        base._Ready();
    }
}