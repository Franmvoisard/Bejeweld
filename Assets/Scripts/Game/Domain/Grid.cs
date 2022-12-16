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
    }
}