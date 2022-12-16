using System;

namespace Shoelace.Bejeweld.Errors
{
    public class TileNotFoundException : Exception
    {
        public TileNotFoundException(int x, int y) : base($"There is any tile at position ({x}, {y})") { }
    }
}