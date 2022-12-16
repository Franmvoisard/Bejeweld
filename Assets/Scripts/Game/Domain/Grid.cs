using Shoelace.Bejeweld.Errors;
using UnityEngine;

namespace Shoelace.Bejeweld
{
    public interface IGrid
    {
        void AddTile(Tile tile);
        Tile Find(int x, int y);
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

        public Tile[,] GetTiles()
        {
            return _tiles;
        }

        public void RemoveTile(Vector2Int cellPosition)
        {
            if (_tiles[cellPosition.x, cellPosition.y] == null) throw new TileNotFoundException(cellPosition.x, cellPosition.y);
            _tiles[cellPosition.x, cellPosition.y] = null;
        }
    }
}