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
            _grid = new Grid(GridRows, GridColumns);
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
        public void Update_Tiles_Grid_Position_When_Swap_Adjacent_Tiles()
        {
            //Given
            var tileOneStartPosition = new Vector2Int(0, 0);
            var tileTwoStartPosition = new Vector2Int(1, 0);

            var tileOne = new Tile(0, 0);
            var tileTwo = new Tile(1, 0);
            
           _grid.AddTile(tileOne);
           _grid.AddTile(tileTwo);
            
            //When
            _grid.SwapTiles(tileOne, tileTwo);

            //Then
            var tileAtTilePositionOne = _grid.Find(tileOneStartPosition);
            var tileAtTilePositionTwo = _grid.Find(tileTwoStartPosition);

            Assert.AreEqual(tileOne, tileAtTilePositionTwo);
            Assert.AreEqual(tileTwo, tileAtTilePositionOne);
            Assert.AreEqual(tileOneStartPosition, tileTwo.GridPosition);
            Assert.AreEqual(tileTwoStartPosition, tileOne.GridPosition);
        }
        
        [TestCase(0,0,2,0)]
        [TestCase(0,0,1,1)]
        [TestCase(3,3,3,1)]
        [TestCase(0,0,4,0)]
        public void Do_Not_Update_Tiles_Grid_Position_When_Swap_NonAdjacent_Tiles(int tileOneStartX, int tileOneStartY,
            int tileTwoStartX, int tileTwoStartY)
        {
            //Given
            var tileOneStartPosition = new Vector2Int(tileOneStartX, tileOneStartX);
            var tileTwoStartPosition = new Vector2Int(tileTwoStartX, tileTwoStartY);

            var tileOne = new Tile(tileOneStartX, tileOneStartY);
            var tileTwo = new Tile(tileTwoStartX, tileTwoStartY);
            
            _grid.AddTile(tileOne);
            _grid.AddTile(tileTwo);
            
            //When
            _grid.SwapTiles(tileOne, tileTwo);

            //Then
            var tileAtTilePositionOne = _grid.Find(tileOneStartPosition);
            var tileAtTilePositionTwo = _grid.Find(tileTwoStartPosition);
            
            Assert.AreEqual(tileOne, tileAtTilePositionOne, "Tile was swapped but not adjacent");
            Assert.AreEqual(tileTwo, tileAtTilePositionTwo, "Tile was swapped but not adjacent");
            Assert.AreEqual(tileOneStartPosition, tileOne.GridPosition);
            Assert.AreEqual(tileTwoStartPosition, tileTwo.GridPosition);
        }

        [Test]
        public void Throw_CannotSwapUnattachedTileException_When_Trying_To_Swap_Tiles_Not_Present_In_The_Grid()
        {
            //Given
            var tileOneStartPosition = new Vector2Int(0, 0);
            var tileTwoStartPosition = new Vector2Int(2, 0);

            var tileOne = new Tile(0, 0);
            var tileTwo = new Tile(2, 0);
            
            //When-Then
            Assert.Throws<CannotSwapUnattachedTileException>(() => _grid.SwapTiles(tileOne, tileTwo));
            Assert.AreEqual(tileOneStartPosition, tileOne.GridPosition);
            Assert.AreEqual(tileTwoStartPosition, tileTwo.GridPosition);
        }
    }

}