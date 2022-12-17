using System;

namespace Shoelace.Bejeweld.Errors
{
    public class TileNotFoundException : Exception
    {
        public int X { get; }
        public int Y { get; }

        public TileNotFoundException(int x, int y) : base($"There is any tile at position ({x}, {y})")
        {
            X = x;
            Y = y;
        }
    }
}