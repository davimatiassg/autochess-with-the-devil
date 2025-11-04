using Godot;
using System;
using System.Collections.Generic;

[GlobalClass]
public partial class TileHighlighter : Node
{
    private static List<TabletopTile> highlightedTiles = new();

    private static List<Material> previousMaterials = new();

    private static TileHighlighter Instance;

    [Export] private Material highlightMaterial;

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
    public override void _Ready()
    {
        base._Ready();
        if (Instance == null) Instance = this;
        else if (Instance != this) { QueueFree(); return; }
    }
}
