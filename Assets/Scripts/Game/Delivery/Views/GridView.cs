using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Shoelace.Bejeweld.Components;
using Shoelace.Bejeweld.Factories;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UIElements.Experimental;

namespace Shoelace.Bejeweld.Views
{
    public partial class GridView : MonoBehaviour
    {
        [Header("Grid Settings")]
        [SerializeField] private Transform tileParent;
        [SerializeField] private Vector2Int gridSize;
        [SerializeField] private int tileSize = 100;
        [SerializeField] private int tileSpacing = 100;
        
        [Header("Tile Settings")]
        [SerializeField] private AbstractTileViewFactory tileViewFactory;
        [SerializeField] private float timeToFall = 0.5f;

        private IGrid _grid;
        private readonly Dictionary<Vector2Int, TileView> _tileViews = new();

        public IMatchFinder MatchFinder { get; private set; }
        public GridState CurrentState { get; private set; }
        private static event Action OnMatchesCleared;
        private static event Action OnRefillComplete;
        private static event Action OnTilesDropped;

        private void Awake()
        {
            ListenToEvents();
        }

        private void ListenToEvents()
        {
            TileSwapper.OnSwapFinished += OnSwapFinished;
            TileSwapper.OnSwapFailed += OnSwapFailed;
            OnRefillComplete += ClearMatches;
            OnMatchesCleared += DropTiles;
            OnTilesDropped += Refill;
        }

        private void OnSwapFailed()
        {
            CurrentState = GridState.Interactable;
        }

        private void Start()
        {
            _grid = new Grid(gridSize.x, gridSize.y);
            MatchFinder = new MatchFinder(_grid);
            _grid.PopulateWithRandomTiles();
            CreateLayout();
        }

        private void OnSwapFinished(TileView tileA, TileView tileB)
        {
            ClearMatches();
        }

        private void CreateLayout()
        {
            for (var column = 0; column < _grid.ColumnCount; column++)
            {
                for (var row = 0; row < _grid.RowCount; row++)
                {
                    var tileView = InstantiateTileView(row, column, out var tilePositionRect);
                    tilePositionRect.anchoredPosition = CalculateTilePosition(row, column);
                    _tileViews[tileView.Tile.GridPosition] = tileView;
                }
            }
        }

        private TileView InstantiateTileView(int row, int column, out RectTransform tilePositionRect)
        {
            var tileView = tileViewFactory.CreateTileView(_grid.Find(row, column));
            tilePositionRect = tileView.GetComponent<RectTransform>();
            tileView.transform.SetParent(tileParent, false);
            return tileView;
        }

        private Vector2 CalculateTilePosition(int row, int column)
        {
            return new Vector2(row * tileSize + tileSpacing * row, column * -tileSize - tileSpacing * column);
        }

        public void Swap(TileView tileA, TileView tileB)
        {
            CurrentState = GridState.Swapping;
            _tileViews[tileA.Tile.GridPosition] = tileB;
            _tileViews[tileB.Tile.GridPosition] = tileA;

            _grid.SwapTiles(tileA.Tile, tileB.Tile);
        }
        
        private void ClearMatches()
        {
            var matchingTiles = GetMatchingTiles();
            if (matchingTiles.Length == 0)
            {
                CurrentState = GridState.Interactable;
                return;
            }
            DestroyAndRemoveTiles(matchingTiles);
            OnMatchesCleared?.Invoke();
        }

        private void DropTiles()
        {
            var displacedTiles = _grid.DropTiles();
            StartCoroutine(DropTiles(displacedTiles));
        }

        private Tile[] GetMatchingTiles()
        {
            return MatchFinder.LookForMatches().SelectMany(x => x.Tiles).Distinct().ToArray();
        }

        private void DestroyAndRemoveTiles(Tile[] tiles)
        {
            for (var i = 0; i < tiles.Length; i++)
            {
                var gridPosition = tiles[i].GridPosition;
                var tileView = _tileViews[gridPosition];
                _grid.RemoveTile(gridPosition);
                Destroy(tileView.gameObject);
                _tileViews[gridPosition] = null;
            }
        }

        private void Refill()
        {
            var tiles = _grid.PopulateEmptyTiles();
            var tilePositions = tiles
                .Select(tile => tile.GridPosition)
                .GroupBy(x => x.x)
                .Select(x => x.OrderByDescending(x => x.y).ThenBy(x => x.x).ToArray())
                .ToArray();
            var refillData = new RefillData[tiles.Length];
            var iterator = 0;
            for (var i = 0; i < tilePositions.Length; i++)
            {
                for (var j = 0; j < tilePositions[i].Length; j++)
                {
                    var tilePos = tilePositions[i][j];
                    var tileView = InstantiateTileView(tilePos.x, tilePos.y, out var rectTransform);
                    _tileViews[tilePos] = tileView;
                    rectTransform.anchoredPosition = CalculateTilePosition(tilePos.x, -1 - j);
                    refillData[iterator] = new RefillData(rectTransform, CalculateTilePosition(tilePos.x, tilePos.y));
                    iterator++;
                }
            }
            StartCoroutine(RefillDrop(refillData));
        }

        private IEnumerator RefillDrop(RefillData[] refillData)
        {
            var time = 0f;
            while (time < timeToFall)
            {
                time += Time.deltaTime;
                var finalPositions = refillData.Select(drop => drop.FinalPosition).ToArray();
                var viewTransforms = refillData.Select(drop => drop.Transform).ToArray();
                
                var easedTime = Easing.OutBounce(time / timeToFall);
                for (var i = 0; i < refillData.Length; i++)
                {
                    viewTransforms[i].anchoredPosition = Vector2.Lerp(viewTransforms[i].anchoredPosition, finalPositions[i], easedTime);
                }
             
                yield return new WaitForEndOfFrame();
            }
            OnRefillComplete?.Invoke();
        }

        private IEnumerator DropTiles(Drop[] drops)
        {
            var time = 0f;
            var finalPositions = CalculateFinalPositions(drops);
            var viewTransforms = GetViewRectTransforms(drops);
            UpdateDroppedTilesGridPosition(drops);
            
            while (time < timeToFall)
            {
                time += Time.deltaTime;
                
                var easedTime = Easing.InQuad(time / timeToFall);
                for (var i = 0; i < drops.Length; i++)
                {
                    viewTransforms[i].anchoredPosition = Vector2.Lerp(viewTransforms[i].anchoredPosition, finalPositions[i], easedTime);
                }
             
                yield return null;
            }

            OnTilesDropped?.Invoke();
        }

        private void UpdateDroppedTilesGridPosition(Drop[] drops)
        {
            for (var i = 0; i < drops.Length; i++)
            {
                _tileViews[drops[i].NewPosition] = _tileViews[drops[i].PreviousPosition];
                _tileViews[drops[i].PreviousPosition] = null;
            }
        }

        private RectTransform[] GetViewRectTransforms(Drop[] drops)
        {
            return drops.Select(drop => _tileViews[drop.PreviousPosition])
                .Select(x => x.GetComponent<RectTransform>())
                .ToArray();
        }

        private Vector2[] CalculateFinalPositions(Drop[] drops)
        {
            return drops.Select(drop => CalculateTilePosition(drop.NewPosition.x, drop.NewPosition.y)).ToArray();
        }


        private void OnDestroy()
        {
            UnsubscribeFromEvents();
        }

        private void UnsubscribeFromEvents()
        {
            TileSwapper.OnSwapFinished -= OnSwapFinished;
            TileSwapper.OnSwapFailed -= OnSwapFailed;
            OnRefillComplete -= ClearMatches;
            OnMatchesCleared -= DropTiles;
            OnTilesDropped -= Refill;
        }

        public bool IsAdjacent(TileView tileView, TileView selectedTile) => _grid.AreAdjacent(tileView.Tile, selectedTile.Tile);
    }

    public enum GridState
    {
        Interactable,
        Swapping
    }
}