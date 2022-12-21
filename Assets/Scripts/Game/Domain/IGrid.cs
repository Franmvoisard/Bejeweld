using System.Collections.Generic;
using UnityEngine;

namespace Shoelace.Bejeweld
{
    public interface IGrid
    {
        int ColumnCount { get; }
        int RowCount { get; }
        void AddTile(Tile tile);
        Tile Find(int x, int y);
        Tile Find(Vector2Int cellPosition);
        Tile[,] GetTiles();
        void RemoveTile(Vector2Int cellPosition);
        void SwapTiles(Tile tileOne, Tile tileTwo);
        void PopulateWithProvidedTiles(params int[] orderedTypes);
        void PopulateWithRandomTiles();
        Drop[] DropTiles();
        Vector2Int[] GetEmptyPositions();
        Tile[] PopulateEmptyTiles();
        bool AreAdjacent(Tile tileViewTile, Tile selectedTileTile);
    }
}