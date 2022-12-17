using UnityEngine;

namespace Shoelace.Bejeweld
{
    public class Tile
    {
        public Vector2Int GridPosition { get;  set; }
        public Tile(int x, int y)
        {
            GridPosition = new Vector2Int(x, y);
        }

        public Tile(Vector2Int gridPosition)
        {
            GridPosition = gridPosition;
        }
    }
}