using NUnit.Framework;
using Shoelace.Bejeweld;
using UnityEngine;

namespace Tests.Editor
{
    public class CellShould
    {
        [Test]
        public void Update_Position_When_Swap_Positions_With_Another_Cell()
        {
            //Given
            var cellOneInitialPosition = new Vector2(1, 1);
            var cellTwoInitialPosition = new Vector2(2, 1);
            
            var cellOne = new Cell(cellOneInitialPosition);
            var cellTwo = new Cell(cellTwoInitialPosition);
            
            //When
            cellOne.Swap(cellTwo);
            
            //Then
            Assert.AreEqual(cellOne.Position,cellTwoInitialPosition);
            Assert.AreEqual(cellTwo.Position,cellOneInitialPosition);
        }
    }
}
