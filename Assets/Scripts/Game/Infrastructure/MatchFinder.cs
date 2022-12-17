using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
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

            var strBuilder = new StringBuilder();
            for (var column = 0; column < _grid.ColumnCount; column++)
            {
                for (var row = 0; row < _grid.RowCount; row++)
                {
                    if (IsEnoughSpaceForHorizontalMatch(row) == false)
                    {
                        strBuilder.Append("[ " + tiles[row, column].TypeId + " ]");
                        strBuilder.Append("[ " + tiles[row + 1, column].TypeId + " ]");
                        break;
                    }
                    
                    // If (C) + (C+1) + (C+2) == type
                    // TODO: Continuar, sumar el resultado y avanzar el iterador para compensar.
                    var currentTile = tiles[row, column];
                    var currentPlusOne = tiles[row + 1, column];
                    var currentPlusTwo = tiles[row + 2, column];
                    strBuilder.Append("[ " + tiles[row, column].TypeId + " ]");

                    if (DoMatchType(currentTile, currentPlusOne, currentPlusTwo))
                    {
                        Debug.Log($"Match: ({row}, {column}), ({row + 1}, {column}), ({row + 2}, {column})");
                        matches.Add(new Match(currentTile, currentPlusOne, currentPlusTwo));
                    }
                }
                strBuilder.Append("\n");
            }
            Debug.Log("Horizontal Look Time: " + stopwatch.Elapsed);
            Debug.Log(strBuilder.ToString());
            stopwatch.Stop();
            return matches.ToArray();
        }

        private bool DoMatchType(params Tile[] tiles)
        {
            var length = tiles.Length;
            var tileType = tiles[0].TypeId;
            
            for (var i = 1; i < length; i++)
            {
                if (tiles[i].TypeId != tileType) return false;
            }
            
            Debug.Log($"Tiles: ({tiles[0].GridPosition}), ({tiles[1].GridPosition}), ({tiles[2].GridPosition}) Match type: {tileType}");
            return true;
        }

        private bool IsEnoughSpaceForHorizontalMatch(int row)
        {
            return row + 2 < _grid.ColumnCount;
        }
    }
}
