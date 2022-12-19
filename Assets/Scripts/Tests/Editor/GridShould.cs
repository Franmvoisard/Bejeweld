using NUnit.Framework;
using Shoelace.Bejeweld;
using Shoelace.Bejeweld.Errors;
using UnityEngine;
using Grid = Shoelace.Bejeweld.Grid;

namespace Tests.Editor
{
    public class GridShould
    {
        private IGrid _grid;
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
            var tileToAdd = new Tile(newTilePosition.x, newTilePosition.y, 0);
            
            //When
            _grid.AddTile(tileToAdd);
            
            //Then
            Assert.NotNull(_grid.Find(newTilePosition.x, newTilePosition.y));
        }

        [Test]
        public void Add_Tile_To_Grid_Throws_An_Exception_If_Space_Is_Already_Occupied()
        {
            var newTilePosition = new Vector2Int(1, 1);
            var tileToAdd = new Tile(newTilePosition.x, newTilePosition.y, 0);
            _grid.AddTile(tileToAdd);
            
            //When-Then
            Assert.Throws<TileAlreadyExistsException>(() => _grid.AddTile(tileToAdd));
        }

        [Test]
        public void Find_A_Tile_Given_A_Position()
        {
            //Given
            var tilePosition = new Vector2Int(2, 2);
            _grid.AddTile(new Tile(tilePosition.x, tilePosition.y, 0));
            
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
            _grid.AddTile(new Tile(tilePosition.x, tilePosition.y,0));

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

            var tileOne = new Tile(0, 0,0);
            var tileTwo = new Tile(1, 0,0);
            
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

            var tileOne = new Tile(tileOneStartX, tileOneStartY,0);
            var tileTwo = new Tile(tileTwoStartX, tileTwoStartY,0);
            
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

            var tileOne = new Tile(0, 0, 0);
            var tileTwo = new Tile(2, 0, 0);
            
            //When-Then
            Assert.Throws<CannotSwapUnattachedTileException>(() => _grid.SwapTiles(tileOne, tileTwo));
            Assert.AreEqual(tileOneStartPosition, tileOne.GridPosition);
            Assert.AreEqual(tileTwoStartPosition, tileTwo.GridPosition);
        }
        
        [Test]
        public void Populate_Grid_With_Random_Tiles()
        {
            //When
            _grid.PopulateWithRandomTiles();
            
            //Then
            Assert.AreEqual(64,_grid.GetTiles().Length);
        }

        [Test]
        public void Populate_Grid_With_Provided_Tiles()
        {
            //Given
            const int rows = 5;
            const int columns = 5;
            _grid = new Grid(rows, columns);
            
            var testTable = new int[]
            {
                0, 0, 1, 1, 1,
                0, 1, 0, 0, 2,
                2, 3, 0, 2, 1,
                0, 2, 0, 0, 0,
                1, 0, 2, 0, 0
            };

            //When
            _grid.PopulateWithProvidedTiles(testTable);
            
            //Then
            var iterator = 0;
            
            for (var i = 0; i < columns; i++)
            {
                for (var j = 0; j < rows; j++)
                {
                    Assert.AreEqual(testTable[iterator],_grid.GetTiles()[j, i].TypeId);
                    iterator++;
                }
            }
        }

        [TestCase(8, 8, new[]
        {
            0, 0, 1, 1, 1, 1, 1, 1,
            0, 1, 0, 0, 2, 0, 0, 2,
            2, 3, 0, 2, 1, 0, 2, 1,
            0, 2, 0, 0, 0, 0, 0, 0,
            1, 0, 2, 0, 0, 2, 0, 0,
            2, 3, 0, 2, 1, 0, 2, 1,
            0, 2, 0, 0, 0, 0, 0, 0,
            1, 0, 2, 0, 0, 2, 0, 0
        }, new int[] { 1,1, 2,3, 3,4, 4,5, 5,6 })]
        [TestCase(5, 5, new[]
        {
            0, 0, 1, 1, 1,
            0, 1, 0, 0, 2,
            2, 3, 0, 2, 1,
            0, 2, 0, 0, 0,
            1, 0, 2, 0, 0
        }, new int[] { 0,0, 1,1, 2,2, 3,3, 4,4 })]
        [TestCase(5, 5, new[]
        {
            0, 0, 1, 1, 1,
            0, 1, 0, 0, 2,
            2, 3, 0, 2, 1,
            0, 2, 0, 0, 0,
            1, 0, 2, 0, 0
        }, new int[] { 0,4, 1,4, 2,4, 3,4, 4,4 })]
        public void Detect_Empty_Spaces(int rows, int columns, int[] grid, int[] emptySpaces)
        {
            //Given
            _grid = new Grid(rows,columns);

            var expectedEmptySpaces = new Vector2Int[emptySpaces.Length/2];
            CreateVector2IntFromPositionsInAnIntArray(emptySpaces, expectedEmptySpaces);
            _grid.PopulateWithProvidedTiles(grid);
            
            //Remove last row
            foreach (var tileSpace in expectedEmptySpaces) _grid.RemoveTile(tileSpace);

            //When
            var result = _grid.GetEmptyPositions();

            //Then
            Assert.AreEqual(expectedEmptySpaces,result);
        }

        [TestCase(5, 5, new[]
        {
            0, 0, 1, 1, 1,
            0, 1, 0, 0, 2,
            2, 3, 0, 2, 1,
            0, 2, 0, 1, 0,
            1, 0, 2, 0, 0
        }, new int[]
        {
            //Matches
            2,0, 3,0, 4,0, 
            2,1, 2,2, 2,3
        }, new[] {
            
            0, 0,-1,-1,-1,
            0, 1,-1, 0, 2,
            2, 3,-1, 2, 1,
            0, 2,-1, 1, 0,
            1, 0, 2, 0, 0
        })]
        [TestCase(5, 5, new[]
        {
            0, 0, 1, 1, 1,
            0, 1, 0, 0, 2,
            2, 3, 0, 2, 1,
            0, 2, 0, 0, 0,
            1, 0, 2, 0, 0 
        }, new int[] { 0,4, 1,4, 2,4, 3,4, 4,4 },  new[] {
            -1,-1,-1,-1,-1,
            0, 0, 1, 1, 1,
            0, 1, 0, 0, 2,
            2, 3, 0, 2, 1,
            0, 2, 0, 0, 0,
        })]
        [TestCase(5, 5, new[]
        {
            1, 1, 1, 1, 1,
            0, 1, 0, 0, 2,
            2, 3, 0, 2, 1,
            0, 2, 0, 0, 0,
            1, 0, 2, 0, 0 
        }, new int[]
        {
            0,0, 1,0, 2,0, 3,0, 4,0,
            2,1, 2,2, 2,3, 3,3, 4,3
        },  new[] {
            -1, -1, -1, -1, -1,
            0, 1, -1, -1, -1,
            2, 3, -1, 0, 2,
            0, 2, -1, 2, 1,
            1, 0, 2, 0, 0 
        })]
        public void Drop_Tiles_To_Fill_Empty_Places(int rows, int columns, int[] grid, int[] spacesPositions, int[] expectedTable)
        {
            //Given
            _grid = new Grid(rows, columns);

            var spacesToRemove = new Vector2Int[spacesPositions.Length/2];
            CreateVector2IntFromPositionsInAnIntArray(spacesPositions, spacesToRemove);
            _grid.PopulateWithProvidedTiles(grid);
            foreach (var space in spacesToRemove) _grid.RemoveTile(space);
            
            //When
            _grid.DropTiles();
            
            //Then
            var iterator = 0;
            for (var i = 0; i < columns; i++)
            {
                for (var j = 0; j < rows; j++)
                {
                    var tile = _grid.GetTiles()[j,i];
                    if(tile == null) Assert.AreEqual(-1, expectedTable[iterator]);
                    else
                    {
                        Assert.AreEqual(expectedTable[iterator], tile.TypeId);
                    }
                    iterator++;
                }
            }
        }

        private static void CreateVector2IntFromPositionsInAnIntArray(int[] emptySpaces, Vector2Int[] expectedEmptySpaces)
        {
            for (int i = 0; i < emptySpaces.Length; i += 2)
            {
                var index = i / 2;
                expectedEmptySpaces[index] = new Vector2Int(emptySpaces[i], emptySpaces[i + 1]);
            }
        }
    }
   
}