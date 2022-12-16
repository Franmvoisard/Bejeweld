using UnityEngine;

namespace Shoelace.Bejeweld
{
    public class Cell
    {
        public Vector2 Position { get; private set; }
        
        public Cell(Vector2 cellOneInitialPosition)
        {
            Position = cellOneInitialPosition;
        }

        public void Swap(Cell cellTwo)
        {
            var auxPosition = Position;
            Position = cellTwo.Position;
            cellTwo.SetPosition(auxPosition);
        }

        private void SetPosition(Vector2 position)
        {
            Position = position;
        }
    }
}