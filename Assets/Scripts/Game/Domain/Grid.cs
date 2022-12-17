using System;
using Shoelace.Bejeweld.Errors;
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

    }

    public class Grid : IGrid
    {
        private readonly Tile[,] _tiles;

        public Grid(int rows, int columns)
        {
            _tiles = new Tile[rows, columns];
        }
        
        public void AddTile(Tile tile)
        {
            if (_tiles[tile.GridPosition.x, tile.GridPosition.y] != null) throw new TileAlreadyExistsException(tile.GridPosition);
            _tiles[tile.GridPosition.x, tile.GridPosition.y] = tile;
        }

        public Tile Find(int x, int y)
        {
            if (_tiles[x, y] == null) throw new TileNotFoundException(x,y);
            return _tiles[x, y];
        }

        public Tile Find(Vector2Int gridPosition)
        {
            var x = gridPosition.x;
            var y = gridPosition.y;
            return Find(x,y);
        }
        
        public Tile[,] GetTiles()
        {
            return _tiles;
        }

        public void RemoveTile(Vector2Int cellPosition)
        {
            if (_tiles[cellPosition.x, cellPosition.y] == null) throw new TileNotFoundException(cellPosition.x, cellPosition.y);
            _tiles[cellPosition.x, cellPosition.y] = null;
        }

        public void SwapTiles(Tile tileOne, Tile tileTwo)
        {
            ValidateTilesAreNotNull(tileOne, tileTwo);
            
            var tileOneGridPosition = tileOne.GridPosition;
            var tileTwoGridPosition = tileTwo.GridPosition;

            if (!AreAdjacent(tileOne, tileTwo)) return;

            tileOne.GridPosition = tileTwoGridPosition;
            tileTwo.GridPosition = tileOneGridPosition;
            _tiles[tileOneGridPosition.x, tileOneGridPosition.y] = tileTwo;
            _tiles[tileTwoGridPosition.x, tileTwoGridPosition.y] = tileOne;
        }

        private void ValidateTilesAreNotNull(Tile tileOne, Tile tileTwo)
        {
            try
            {
                Find(tileOne.GridPosition);
                Find(tileTwo.GridPosition);
            }
            catch (TileNotFoundException exception)
            {
                throw new CannotSwapUnattachedTileException(
                    $"Tile at ({exception.X}, {exception.Y})  is not present in the grid.");
            }
        }

        private bool AreAdjacent(Tile tileOne, Tile tileTwo)
        {
            return Math.Abs(Vector2Int.Distance(tileOne.GridPosition, tileTwo.GridPosition) - 1) < 0.00001f;
        }
    }
}