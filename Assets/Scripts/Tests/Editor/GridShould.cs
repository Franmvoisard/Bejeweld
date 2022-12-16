using NSubstitute;
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
        private const int GridRows = 8;
        private const int GridColumns = 8;

        [SetUp]
        public void Setup()
        {
            _grid = new Grid(GridRows,GridColumns);
        }
        
        [Test]
        public void Add_Tile_To_Grid()
        {
            //Given
            var newTilePosition = new Vector2Int(0, 0);
            var tileToAdd = new Tile(newTilePosition.x, newTilePosition.y);
            
            //When
            _grid.AddTile(tileToAdd);
            
            //Then
            Assert.NotNull(_grid.Find(newTilePosition.x, newTilePosition.y));
        }

        [Test]
        public void Add_Tile_To_Grid_Throws_An_Exception_If_Space_Is_Already_Occupied()
        {
            var newTilePosition = new Vector2Int(1, 1);
            var tileToAdd = new Tile(newTilePosition.x, newTilePosition.y);
            _grid.AddTile(tileToAdd);
            
            //When-Then
            Assert.Throws<TileAlreadyExistsException>(() => _grid.AddTile(tileToAdd));
        }

        [Test]
        public void Find_A_Tile_Given_A_Position()
        {
            //Given
            var tilePosition = new Vector2Int(2, 2);
            _grid.AddTile(new Tile(tilePosition.x, tilePosition.y));
            
            //When
            var result = _grid.Find(tilePosition.x, tilePosition.y);
            
            //Then
            Assert.NotNull(result);
            Assert.AreEqual(tilePosition, result.GridPosition);
        }
        
        [Test]
        public void Throw_TileNotFoundException_When_Trying_To_Find_A_Non_Existent_Tile()
        {
            //Given
            var emptyTileSlotPosition = new Vector2Int(1, 1);
            
            //When-Then
            Assert.Throws<TileNotFoundException>(() => _grid.Find(emptyTileSlotPosition.x, emptyTileSlotPosition.y));
        }

        [Test]
        public void Remove_A_Tile_That_Exists_By_Its_Grid_Position()
        {
            //Given
            var tilePosition = new Vector2Int(2, 2);
            _grid.AddTile(new Tile(tilePosition.x, tilePosition.y));

            Assert.IsNotNull(_grid.Find(tilePosition.x, tilePosition.y));
            
            //When
            _grid.RemoveTile(new Vector2Int(2, 2));
            
            //Then
            Assert.Throws<TileNotFoundException>(() => _grid.Find(tilePosition.x, tilePosition.y));
        } 
        
        [Test]
        public void Throw_TileNotFoundException_When_RemoveTile_But_No_Tile_At_The_Given_Position()
        {
            //When-Then
            Assert.Throws<TileNotFoundException>(() => _grid.RemoveTile(new Vector2Int(2, 2)));
        }

        [Test]
        public void Swap_Adjacent_Tiles()
        {
            //Given

            //When
            //_grid.Swap();

            //Then
            //Assert.AreEqual();
        }
    }

}