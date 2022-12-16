using System.Collections.Generic;
using System.Linq;
using Shoelace.Bejeweld.Errors;
using UnityEngine;

namespace Shoelace.Bejeweld
{
    public class Grid
    {
        private readonly Dictionary<Vector2Int, Cell> _cells = new Dictionary<Vector2Int, Cell>();

        public void AddCell(Cell cell)
        {
            _cells[cell.GridPosition] = cell;
        }

        public Cell Find(Vector2Int cellPosition)
        {
            if (_cells.ContainsKey(cellPosition) == false) throw new CellNotFoundException(cellPosition);
            return _cells[cellPosition];
        }

        public Cell[] GetCells()
        {
            return _cells.Values.ToArray();
        }

        public void RemoveCell(Vector2Int cellPosition)
        {
            if (_cells.ContainsKey(cellPosition) == false) throw new CellNotFoundException(cellPosition);
            _cells.Remove(cellPosition);
        }
    }
}