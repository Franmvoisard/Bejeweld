using UnityEngine;

namespace Shoelace.Bejeweld
{
    public interface IGrid
    {
        void AddTile(Tile tile);
        Tile Find(int x, int y);
        Tile Find(Vector2Int cellPosition);
        Tile[,] GetTiles();
        void RemoveTile(Vector2Int cellPosition);
        int ColumnCount { get; }
        int RowCount { get; }

        void SwapTiles(Tile tileOne, Tile tileTwo);
        void PopulateWithProvidedTiles(params int[] orderedTypes);
        void PopulateWithRandomTiles();
        Vector2Int[] GetEmptyPositions();
    }
}