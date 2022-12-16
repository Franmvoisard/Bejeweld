using System;
using UnityEngine;

namespace Shoelace.Bejeweld.Errors
{
    public class CellNotFoundException : Exception
    {
        public CellNotFoundException(Vector2Int position) : base($"There is any cell at position ({position.x}, {position.y})") { }
    }
}