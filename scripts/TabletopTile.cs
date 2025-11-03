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
        return true;
    }

    public Creature containsCreature()
    {
        foreach (var obj in objectsInThisTile) if (obj is Creature creature) return creature;
        return null;
    }

    public override void _Ready()
    {
        base._Ready();
    }
}