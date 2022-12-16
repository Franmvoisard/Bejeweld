using System;
using UnityEngine;

namespace Shoelace.Bejeweld.Errors
{
    public class CellAlreadyExistsException : Exception
    {
        public CellAlreadyExistsException(Vector2Int position) : base($"Cannot add a cell. A cell already exists in position ({position.x}, {position.y})")
        {
        }
    }
}