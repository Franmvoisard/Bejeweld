using NUnit.Framework;
using Shoelace.Bejeweld;

namespace Tests.Editor
{
    public class MatchFinderShould
    {
        private IMatchFinder _matchFinder;
        private IGrid _grid;

        [SetUp]
        public void Setup()
        {
            _matchFinder = new MatchFinder(_grid);
        }
    
        [TestCase(3,3,new[]
        {
            0, 1, 1,
            0, 2, 0,
            1, 4, 3
        }, 0)]
        [TestCase(5,5,new[]
        {
            0, 1, 1, 1, 0, //Match (1,1,1)
            0, 2, 0, 1, 3,
            1, 4, 3, 3, 3, //Match (3,3,3)
            2, 2, 3, 2, 3,
            3, 3, 3, 2, 1  //Match (3,3,3)
        }, 3)]
        [TestCase(5,5,new[]
        {
            0, 1, 1, 1, 0, //Match (1,1,1)
            0, 2, 0, 1, 3,
            1, 4, 3, 0, 3,
            2, 2, 1, 2, 3,
            0, 2, 0, 2, 1
        }, 1)]
        public void Find_Horizontal_Matches_Of_3(int rows, int columns, int[] grid, int expectedMatches)
        {
            //Given
            _grid = new Grid(rows, columns);
     
            _grid.PopulateWithProvidedTiles(grid);
            _matchFinder = new MatchFinder(_grid);
        
            //When
            var result = _matchFinder.LookForMatches();
        
            //Then
            Assert.AreEqual(expectedMatches, result.Length);
        }
        
        [TestCase(5,5,new[]
        {
            0, 1, 1, 1, 1, //Match (1,1,1,1)
            0, 2, 0, 1, 3,
            1, 4, 3, 3, 3, //Match (3,3,3)
            2, 2, 3, 2, 3,
            3, 3, 3, 3, 3  //Match (3,3,3,3,3)
        }, 3)]
        [TestCase(5,5,new[]
        {
            0, 1, 1, 1, 1, //Match (1,1,1)
            0, 2, 0, 1, 3,
            1, 4, 3, 0, 3,
            2, 2, 1, 2, 3,
            0, 2, 0, 2, 1
        }, 1)]
        [TestCase(8,8,new[]
        {
            1, 1, 1, 1, 1, 1, 1, 1, //Match (1,1,1,1,1,1,1,1)
            0, 2, 0, 1, 3, 3, 3, 3, //Match (3,3,3,3)
            1, 4, 3, 2, 2, 2, 1, 1, //Match (2,2,2)
            2, 2, 1, 2, 3, 1, 1, 1, //Match (1,1,1)
            0, 2, 0, 0, 0, 1, 1, 1, //Match (0,0,0) (1,1,1)
            0, 2, 0, 1, 0, 1, 0, 1,
            0, 2, 0, 1, 0, 1, 3, 1,
            0, 2, 0, 2, 0, 1, 2, 1
        }, 6)]
        public void Find_Horizontal_Matches_Of_3_Or_More(int rows, int columns, int[] grid, int expectedMatches)
        {
            //Given
            _grid = new Grid(rows, columns);
     
            _grid.PopulateWithProvidedTiles(grid);
            _matchFinder = new MatchFinder(_grid);
        
            //When
            var result = _matchFinder.LookForMatches();
        
            //Then
            Assert.AreEqual(expectedMatches, result.Length);
        }
        
    }
}