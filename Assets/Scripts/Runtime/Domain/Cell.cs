using System;
using UnityEngine;

namespace Shoelace.Bejeweld
{
    public class Cell
    {
        public Vector2Int GridPosition { get; private set; }
        
        public Cell(Vector2Int cellOneInitialGridPosition)
        {
            GridPosition = cellOneInitialGridPosition;
        }

        public void Swap(Cell target)
        {
            if (IsValidPosition(target) == false)
            {
                throw new ArgumentException($"Cannot swap to a position with negative values. ({target.GridPosition})");
            }
            
            var auxPosition = GridPosition;
            SetPosition(target.GridPosition);
            target.SetPosition(auxPosition);
        }

        private static bool IsValidPosition(Cell target)
        {
            return target.GridPosition.x > 0 && target.GridPosition.y > 0;
        }

        private void SetPosition(Vector2Int position)
        {
            GridPosition = position;
        }
    }
}