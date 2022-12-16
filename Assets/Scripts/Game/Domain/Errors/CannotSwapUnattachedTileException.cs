using System;

namespace Shoelace.Bejeweld.Errors
{
    public class CannotSwapUnattachedTileException : Exception
    {
        public CannotSwapUnattachedTileException(string message) : base(message)
        {
        }
    }
}