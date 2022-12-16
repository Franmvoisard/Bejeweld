using NUnit.Framework;
using Shoelace.Bejeweld;
using Shoelace.Bejeweld.Errors;
using UnityEngine;
using Grid = Shoelace.Bejeweld.Grid;

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

        [Test]
        public void Find_A_Cell_Given_A_Position()
        {
            //Given
            var cellPosition = new Vector2Int(2, 2);
            _grid.AddCell(new Cell(cellPosition));
            
            //When
            var result = _grid.Find(cellPosition);
            
            //Then
            Assert.NotNull(result);
            Assert.AreEqual(cellPosition, result.GridPosition);
        }
        
        [Test]
        public void Throw_CellNotFoundException_When_Trying_To_Find_A_Non_Existent_Cell()
        {
            //Given
            var emptyCellSlotPosition = new Vector2Int(1, 1);
            
            //When-Then
            Assert.Throws<CellNotFoundException>(() => _grid.Find(emptyCellSlotPosition));
        }
    }

}