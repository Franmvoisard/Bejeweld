using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Shoelace.Bejeweld.Components;
using Shoelace.Bejeweld.Factories;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

namespace Shoelace.Bejeweld.Views
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
        private readonly Dictionary<Vector2Int, TileView> _tileViews = new Dictionary<Vector2Int, TileView>();
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
        private void OnSwapFinished(TileView tileA, TileView tileB)
        {
            _tileViews[tileA.Tile.GridPosition] = tileB;
            _tileViews[tileB.Tile.GridPosition] = tileA;
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
            var matches = _matchFinder.LookForMatches().SelectMany(x => x.Tiles).Distinct().ToArray();
            for (var i = 0; i < matches.Length; i++)
            {
                var gridPosition = matches[i].GridPosition;
                Debug.Log(gridPosition);

                var tileView = _tileViews[gridPosition];
                _tileViews.Remove(gridPosition);
                _grid.RemoveTile(gridPosition);
                Destroy(tileView.gameObject);
            }

            var displacedTiles = _grid.DropTiles();
            StartCoroutine(DropTiles(displacedTiles));
        }

        private IEnumerator DropTiles(Drop[] drops)
        {
            var time = 0f;
            while (time < 0.8f)
            {
                time += Time.deltaTime;
                var finalPositions = drops.Select(drop => CalculateTilePosition(drop.NewPosition.x, drop.NewPosition.y)).ToArray();
                var viewTransforms = drops.Select(drop => _tileViews[drop.PreviousPosition])
                    .Select(x => x.GetComponent<RectTransform>()).ToArray();
                var easedTime = Easing.InOutQuad(time / 0.8f);
                for (var i = 0; i < drops.Length; i++)
                {
                    viewTransforms[i].anchoredPosition = Vector3.Lerp(viewTransforms[i].anchoredPosition, finalPositions[i], easedTime);
                }
             
                yield return new WaitForEndOfFrame();
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