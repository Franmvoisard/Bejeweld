using System;
using NUnit.Framework;
using Shoelace.Bejeweld;
using UnityEngine;

namespace Tests.Editor
{
    public class CellShould
    {
        [Test]
        public void Update_Position_When_Swap_Position_With_Another_Cell()
        {
            //Given
            var cellOneInitialPosition = new Vector2Int(1, 1);
            var cellTwoInitialPosition = new Vector2Int(2, 1);
            
            var cellOne = new Cell(cellOneInitialPosition);
            var cellTwo = new Cell(cellTwoInitialPosition);
            
            //When
            cellOne.Swap(cellTwo);
            
            //Then
            Assert.AreEqual(cellOne.GridPosition,cellTwoInitialPosition);
            Assert.AreEqual(cellTwo.GridPosition,cellOneInitialPosition);
        }
        
        [Test]
        public void Throw_ArgumentException_When_Attempt_To_Swap_To_A_Negative_Position()
        {
            //Given
            var cellInitialPosition = new Vector2Int(1, 1);
            var cell = new Cell(cellInitialPosition);
            var cellTwo = new Cell(new Vector2Int(-1,1));
            
            //Then
            Assert.Throws<ArgumentException>(() =>
            {
                //When
                cell.Swap(cellTwo);
            });
        }
    }
}
