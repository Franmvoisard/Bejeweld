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
        public void Add_Cell_To_Grid_Throws_An_Exception_If_Space_Is_Already_Occupied()
        {
            var newCellPosition = new Vector2Int(1, 1);
            var cellToAdd = new Cell(newCellPosition);
            _grid.AddCell(cellToAdd);
            
            Assert.AreEqual(1, _grid.GetCells().Length);
            
            //When-Then
            Assert.Throws<CellAlreadyExistsException>(() => _grid.AddCell(cellToAdd));
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

        [Test]
        public void Remove_A_Cell_That_Exists_By_Its_Grid_Position()
        {
            //Given
            var cellPosition = new Vector2Int(2, 2);
            _grid.AddCell(new Cell(cellPosition));

            Assert.IsNotNull(_grid.Find(cellPosition));
            
            //When
            _grid.RemoveCell(new Vector2Int(2, 2));
            
            //Then
            Assert.Throws<CellNotFoundException>(() => _grid.Find(cellPosition));
            Assert.IsEmpty(_grid.GetCells());
        }
        
        [Test]
        public void Throw_CellNotFoundException_When_RemoveCell_But_No_Cell_At_The_Given_Position()
        {
            //When-Then
            Assert.Throws<CellNotFoundException>(() => _grid.RemoveCell(new Vector2Int(2, 2)));
        }
    }

}