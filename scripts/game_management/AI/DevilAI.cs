using Godot;
using System;
using System.Collections.Generic;

public class DevilAI
{
    private CardEffect _bestCardToPlay;
    private TabletopTile _bestTileToPlay;
    private int _bestMoveScore;

    /// <summary>
    /// Decision method of AI, the 'brain'
    /// </summary>
    /// <param name="hand">Cards on the IA Hand</param>
    /// <param name="playableTiles">The tiles that the IA can play</param>
    /// <returns>BestMove if found, or null</returns>
    public BestMove FindBestMove(List<CardEffect> hand, List<TabletopTile> playableTiles)
    {
        _bestCardToPlay = null;
        _bestTileToPlay = null;
        _bestMoveScore = int.MinValue;

        //Found targets
        List<TabletopTile> playerTargetTiles = FindPlayerTargets();

        //Evalue plays
        if (playerTargetTiles.Count > 0)
        {
            //Reative mode (target found)
            EvaluateReactiveMoves(hand, playableTiles, playerTargetTiles);
        }
        else
        {
            //Fallback mode (no targets)
            EvaluateFallbackMoves(hand, playableTiles);
        }

        // 4. Retornar a Decisão
        if (_bestCardToPlay != null)
        {
            return new BestMove(_bestCardToPlay, _bestTileToPlay);
        }

        return null; //No play found
    }

    /// <summary>
    /// Search for player creatures on the board
    /// </summary>
    private List<TabletopTile> FindPlayerTargets()
    {
        List<TabletopTile> playerTargetTiles = new List<TabletopTile>();
        for (int y = 0; y < Tabletop.Instance.boardHeight; y++)
        {
            for (int x = 0; x < Tabletop.Instance.boardWidth; x++)
            {
                TabletopTile tile = Tabletop.table[x][y];
                if (tile == null) continue;

                Creature creatureOnTile = tile.containsCreature();
                if (creatureOnTile != null && creatureOnTile.isPlayerObject)
                {
                    playerTargetTiles.Add(tile);
                }
            }
        }
        return playerTargetTiles;
    }

    /// <summary>
    /// Update the best plays variables, card, tile and score.
    /// </summary>
    private void EvaluateReactiveMoves(List<CardEffect> hand, List<TabletopTile> playableTiles, List<TabletopTile> playerTargetTiles)
    {
        foreach (var card in hand)
        {
            foreach (var spawnTile in playableTiles)
            {
                if (!spawnTile.IsTileValid(card))
                {
                    continue;
                }

                int currentMoveScore = EvaluateMove(card, spawnTile, playerTargetTiles);

                if (currentMoveScore > _bestMoveScore)
                {
                    _bestMoveScore = currentMoveScore;
                    _bestCardToPlay = card;
                    _bestTileToPlay = spawnTile;
                }
            }
        }
    }

    /// <summary>
    /// When no aim found, make the decision based on card atributtes.
    /// </summary>
    private void EvaluateFallbackMoves(List<CardEffect> hand, List<TabletopTile> playableTiles)
    {
        foreach (var card in hand)
        {
            foreach (var spawnTile in playableTiles)
            {
                if (spawnTile.IsTileValid(card))
                {
                    int fallbackScore = GetCardPower(card);
                    if (fallbackScore > _bestMoveScore)
                    {
                        _bestMoveScore = fallbackScore;
                        _bestCardToPlay = card;
                        _bestTileToPlay = spawnTile;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Calculates the score based on manhatthan distance
    /// </summary>
    private int EvaluateMove(CardEffect card, TabletopTile spawnTile, List<TabletopTile> targetTiles)
    {
        int minDistance = int.MaxValue;
        foreach (var targetTile in targetTiles)
        {
            int distance = Mathf.Abs(spawnTile.tilePosition.X - targetTile.tilePosition.X) +
                           Mathf.Abs(spawnTile.tilePosition.Y - targetTile.tilePosition.Y);
            if (distance < minDistance)
            {
                minDistance = distance;
            }
        }

        int score = 100 - minDistance;
        score += GetCardPower(card);
        return score;
    }

    /// <summary>
    /// Calculates the power of cards to avoid draws
    /// </summary>
    private int GetCardPower(CardEffect card)
    {
        if (card is SpawnCreatureCardEffect spawnEffect)
        {
            if (spawnEffect.creatureData != null)
            {
                return spawnEffect.creatureData.hp + spawnEffect.creatureData.damage;
            }
        }
        return 1;
    }
}