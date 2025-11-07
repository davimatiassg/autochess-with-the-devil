using Godot;

/// <summary>
/// record to store armazenar the decision result.
/// Contains the card that will be played and the tile
/// </summary>
public record BestMove(CardEffect CardToPlay, TabletopTile TileToPlayOn);