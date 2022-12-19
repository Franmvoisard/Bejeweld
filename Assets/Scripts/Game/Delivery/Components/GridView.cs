using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Shoelace.Bejeweld.Factories;
using Shoelace.Bejeweld.Views;
using UnityEngine;

namespace Shoelace.Bejeweld.Components
{
    public class GridView : MonoBehaviour
    {
        [Header("Grid Settings")]
        [SerializeField] private Transform tileParent;
        [SerializeField] private Vector2Int gridSize;
        [SerializeField] private int tileSize = 100;
        [SerializeField] private int tileSpacing = 100;
        
        [Header("Tile Settings")]
        [SerializeField] private AbstractTileViewFactory tileViewFactory;
        
        private IGrid _grid;
        private IMatchFinder _matchFinder;
        private Dictionary<Vector2Int, TileView> _tileViews = new Dictionary<Vector2Int, TileView>();
        public GridState CurrentState;

        private void Start()
        {
            _grid = new Grid(gridSize.x, gridSize.y);
            _matchFinder = new MatchFinder(_grid);
            _grid.PopulateWithRandomTiles();
            CreateLayout();
            
            TileSwapper.OnSwapFinished += OnSwapFinished;
        }

        private void OnSwapStarted() => CurrentState = GridState.Swapping;
        private void OnSwapFinished()
        {
            ClearMatches();
            CurrentState = GridState.Interactable;
        }

        public void CreateLayout()
        {
            for (var column = 0; column < _grid.ColumnCount; column++)
            {
                for (var row = 0; row < _grid.RowCount; row++)
                {
                    var tileView = tileViewFactory.CreateTileView(_grid.Find(row, column));
                    var tilePositionRect = tileView.GetComponent<RectTransform>();

                    tileView.transform.SetParent(tileParent, false);
                    tilePositionRect.anchoredPosition = CalculateTilePosition(row, column);
                    _tileViews[tileView.Tile.GridPosition] = tileView;
                }
            }
        }

        private Vector2 CalculateTilePosition(int row, int column)
        {
            return new Vector2(row * tileSize + tileSpacing * row, column * -tileSize - tileSpacing * column);
        }

        public void Swap(Tile tileA, Tile tileB)
        {
            CurrentState = GridState.Swapping;
           _grid.SwapTiles(tileA, tileB);
        }

        private void ClearMatches()
        {
            var matches = _matchFinder.LookForMatches().SelectMany(x => x.Tiles).ToArray();
            for (var i = 0; i < matches.Length; i++)
            {
                var tileView = _tileViews[matches[i].GridPosition];
                _tileViews[matches[i].GridPosition] = null;
                Destroy(tileView.gameObject);
            }
        }
        private void OnDestroy()
        {
            TileSwapper.OnSwapFinished -= OnSwapFinished;
        }
    }

    public enum GridState
    {
        Interactable,
        Swapping,
        Chaining,
        Filling
    }
}