using Godot;
using System;
using System.Collections.Generic;

[GlobalClass]
public partial class TileHighlighter : Node
{
    private static List<TabletopTile> highlightedTiles = new();

    private static List<Material> previousMaterials = new();

    private static TileHighlighter Instance;

    private static TabletopTile ultraHighlighted;

    [Export] private Material highlightMaterial;
    

    [Export] private Material ultraHighlightMaterial;

    public static void ToggleOn(Card card)
    {
        highlightedTiles = new();
        previousMaterials = new();
        foreach (var tile in Tabletop.GetPlaceablePositions(card.effect))
        {
            highlightedTiles.Add(tile);
            var mesh = tile.GetNode<MeshInstance3D>("MeshInstance3D");
            previousMaterials.Add(mesh.MaterialOverlay);
            mesh.MaterialOverlay = Instance.highlightMaterial;

        }
    }

    public static void ToggleOff()
    {
        for (int i = 0; i < highlightedTiles.Count; i++)
        {
            highlightedTiles[i].GetNode<MeshInstance3D>("MeshInstance3D").MaterialOverlay = previousMaterials[i];
        }
    }


    public static void HighlightRaycast(Godot.Collections.Dictionary raycastResults)
    {


        if (raycastResults.Count > 0)
        {

            if (ultraHighlighted != null)
            {
                ultraHighlighted.GetNode<MeshInstance3D>("MeshInstance3D").MaterialOverlay = Instance.highlightMaterial;
                ultraHighlighted = null;
            }
            var tile = ((Node3D)raycastResults["collider"]) as TabletopTile;
            if (tile == null) return;
            if (!highlightedTiles.Contains(tile)) return;

            ultraHighlighted = tile;
            ultraHighlighted.GetNode<MeshInstance3D>("MeshInstance3D").MaterialOverlay = Instance.ultraHighlightMaterial;

        }
        else
        {
            ultraHighlighted.GetNode<MeshInstance3D>("MeshInstance3D").MaterialOverlay = Instance.highlightMaterial;
        }
    }

    public override void _Ready()
    {
        base._Ready();
        if (Instance == null) Instance = this;
        else if (Instance != this) { QueueFree(); return; }
    }
}
