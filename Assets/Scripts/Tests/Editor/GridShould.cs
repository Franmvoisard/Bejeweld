using NUnit.Framework;
using Shoelace.Bejeweld;
using Shoelace.Bejeweld.Errors;
using UnityEngine;

namespace Tests.Editor
{
    public class GridShould
    {
        private Grid _grid;

        [SetUp]
        public void Setup()
        {
            _grid = new Grid();
        }
        
        [Test]
        public void Add_Cell_To_Grid()
        {
            //Given
            var newCellPosition = new Vector2Int(1, 1);
            var cellToAdd = new Cell(newCellPosition);
            
            //When
            _grid.AddCell(cellToAdd);
            
            //Then
            Assert.AreEqual(1,_grid.GetCells().Length);
            Assert.NotNull(_grid.Find(newCellPosition));
        }

    }

}