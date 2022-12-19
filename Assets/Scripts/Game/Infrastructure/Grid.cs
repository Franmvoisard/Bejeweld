//#define LOGGING
using System;
using System.Collections.Generic;
using Shoelace.Bejeweld.Errors;
using UnityEngine;

namespace Shoelace.Bejeweld
{
    public class Grid : IGrid
    {
        private readonly Tile[,] _tiles;
        public int ColumnCount { get; }
        public int RowCount { get; }

        public Grid(int rows, int columns)
        {
            RowCount = rows;
            ColumnCount = columns;
            _tiles = new Tile[rows, columns];
        }
        
        public void AddTile(Tile tile)
        {
            if (_tiles[tile.GridPosition.x, tile.GridPosition.y] != null) throw new TileAlreadyExistsException(tile.GridPosition);
            _tiles[tile.GridPosition.x, tile.GridPosition.y] = tile;
        }

        public Tile Find(int x, int y)
        {
            if (_tiles[x, y] == null) throw new TileNotFoundException(x,y);
            return _tiles[x, y];
        }

        public Tile Find(Vector2Int gridPosition)
        {
            var x = gridPosition.x;
            var y = gridPosition.y;
            return Find(x,y);
        }
        
        public Tile[,] GetTiles()
        {
            return _tiles;
        }

        public void RemoveTile(Vector2Int cellPosition)
        {
            if (_tiles[cellPosition.x, cellPosition.y] == null) throw new TileNotFoundException(cellPosition.x, cellPosition.y);
            _tiles[cellPosition.x, cellPosition.y] = null;
        }

        public void SwapTiles(Tile tileOne, Tile tileTwo)
        {
            ValidateTilesAreNotNull(tileOne, tileTwo);
            
            var tileOneGridPosition = tileOne.GridPosition;
            var tileTwoGridPosition = tileTwo.GridPosition;

            if (!AreAdjacent(tileOne, tileTwo)) return;

            tileOne.GridPosition = tileTwoGridPosition;
            tileTwo.GridPosition = tileOneGridPosition;
            _tiles[tileOneGridPosition.x, tileOneGridPosition.y] = tileTwo;
            _tiles[tileTwoGridPosition.x, tileTwoGridPosition.y] = tileOne;
        }

        private void ValidateTilesAreNotNull(Tile tileOne, Tile tileTwo)
        {
            try
            {
                Find(tileOne.GridPosition);
                Find(tileTwo.GridPosition);
            }
            catch (TileNotFoundException exception)
            {
                throw new CannotSwapUnattachedTileException(
                    $"Tile at ({exception.X}, {exception.Y})  is not present in the grid.");
            }
        }

        private bool AreAdjacent(Tile tileOne, Tile tileTwo)
        {
            return Math.Abs(Vector2Int.Distance(tileOne.GridPosition, tileTwo.GridPosition) - 1) < 0.00001f;
        }

        public void PopulateWithRandomTiles()
        {
            DoForColumnsAndRows(AddRandomTile);
        }
        
        public Vector2Int[] GetEmptyPositions()
        {
            List<Vector2Int> emptyPositions = new List<Vector2Int>();
            for (int column = 0; column < ColumnCount; column++)
            {
                for (int row = 0; row < RowCount; row++)
                {
                    if (_tiles[row, column] == null) emptyPositions.Add(new Vector2Int(row,column));
                }
            }

            return emptyPositions.ToArray();
        }

        public void DropTiles()
        {
            #if LOGGING
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            #endif
            for (var columns = ColumnCount - 1; columns >= 0; columns--) 
            {
                for (var rows = 0; rows < RowCount; rows++)
                {
                    #if LOGGING                    
                    Debug.Log($"({rows}, {columns})");
                    #endif

                    if (_tiles[rows, columns] != null) continue;
                    
                    var columnIndex = columns;
                    while (columnIndex > 0 && _tiles[rows, columnIndex] == null)
                    {
                        columnIndex -= 1;
                    }

                    if (columnIndex == 0 && _tiles[rows, columnIndex] == null) continue;
                    _tiles[rows, columns] = _tiles[rows, columnIndex];
                    _tiles[rows, columns].GridPosition = new Vector2Int(rows, columns);
                    _tiles[rows, columnIndex] = null;
                }
            }
            #if LOGGING
            Debug.Log("Dropping tiles: " + stopwatch.Elapsed);
            stopwatch.Stop();
            #endif
        }
        private void Fall(Tile tile)
        {
            _tiles[tile.GridPosition.x, tile.GridPosition.y + 1] = tile;
            tile.GridPosition = new Vector2Int(tile.GridPosition.x, tile.GridPosition.y + 1);
        }
        public void PopulateWithProvidedTiles(params int[] orderedTypes)
        {
            var iterator = 0;
            for (var i = 0; i < ColumnCount; i++)
            {
                for (var j = 0; j < RowCount; j++)
                {
                    var tile = new Tile(j, i, orderedTypes[iterator]);
                    AddTile(tile);
                    iterator++;
                }
            }
        }

        private void AddRandomTile(int column, int row)  
        {
            AddTile(new Tile(row, column, UnityEngine.Random.Range(0,5)));
        }

        private void DoForColumnsAndRows(Action<int, int> action)
        {
            for (var columns = 0; columns < ColumnCount; columns++)
            {
                for (var rows = 0; rows < RowCount; rows++)
                {
                    action(columns, rows);
                }
            }
        }
    }
}