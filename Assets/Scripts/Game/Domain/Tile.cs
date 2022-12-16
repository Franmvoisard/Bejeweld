using UnityEngine;

namespace Shoelace.Bejeweld
{
    public class Tile
    {
        public Vector2Int GridPosition { get;  set; }
        public int TypeId { get; }

        public Tile(int x, int y, int typeId)
        {
            GridPosition = new Vector2Int(x, y);
            TypeId = typeId;
        }

        public Tile(Vector2Int gridPosition, int typeId) : this(gridPosition.x, gridPosition.y, typeId) {}
    }
}