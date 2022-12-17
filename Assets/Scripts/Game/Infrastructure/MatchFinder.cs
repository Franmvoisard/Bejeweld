using System.Collections.Generic;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

namespace Shoelace.Bejeweld
{
    public class MatchFinder : IMatchFinder
    {
        private readonly IGrid _grid;

        public MatchFinder(IGrid grid)
        {
            _grid = grid;
        }

        public Match[] LookForMatches()
        {
            var tiles = _grid.GetTiles();
            var stopwatch = Stopwatch.StartNew();
            var matches = new List<Match>();

            //var strBuilder = new StringBuilder();
            for (var column = 0; column < _grid.ColumnCount; column++)
            {
                for (var row = 0; row < _grid.RowCount; row++)
                {
                    var currentTypeId = tiles[row, column].TypeId;
                    if (IsEnoughSpaceForHorizontalMatch(row) == false) break;

                    var currentTile = tiles[row, column];
                    var currentPlusOne = tiles[row + 1, column];
                    var currentPlusTwo = tiles[row + 2, column];

                    if (MatchTileType(currentTile, currentPlusOne, currentPlusTwo))
                    {
                        var match = new Match();
                        match.Append(currentTile, currentPlusOne, currentPlusTwo);
                        
                        var additionalMatches = 0;
                        for (var i = row + 3; i <_grid.ColumnCount-row; i++)
                        {
                            var tile = tiles[row + i, column];
                            if (tile.TypeId != currentTypeId) break;
                            
                            match.Append(tile);
                            additionalMatches++;
                        }
                        matches.Add(match);
                        row += 2 + additionalMatches;
                    }
                }
            }

            foreach (var match in matches)
            {
                Debug.Log(match.ToString());
            }
            Debug.Log("Horizontal Look Time: " + stopwatch.Elapsed);
            stopwatch.Stop();
            return matches.ToArray();
        }

        private bool MatchTileType(params Tile[] tiles)
        {
            var length = tiles.Length;
            var tileType = tiles[0].TypeId;
            
            for (var i = 1; i < length; i++)
            {
                if (tiles[i].TypeId != tileType) return false;
            }
            return true;
        }

        private bool IsEnoughSpaceForHorizontalMatch(int row)
        {
            return row + 2 < _grid.ColumnCount;
        }
    }
}
