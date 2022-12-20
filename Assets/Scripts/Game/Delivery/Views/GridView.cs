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
    public class GridView : MonoBehaviour
    {
        [Header("Grid Settings")]
        [SerializeField] private Transform tileParent;
        [SerializeField] private Vector2Int gridSize;
        [SerializeField] private int tileSize = 100;
        [SerializeField] private int tileSpacing = 100;
        
        [Header("Tile Settings")]
        [SerializeField] private AbstractTileViewFactory tileViewFactory;
        [SerializeField] private float timeToFall = 0.5f;

        [Header("Debug")] 
        [SerializeField] private TMP_Text State;
        
        private IGrid _grid;
        private IMatchFinder _matchFinder;
        private readonly Dictionary<Vector2Int, TileView> _tileViews = new Dictionary<Vector2Int, TileView>();
        private GridState _currentState;

        public GridState CurrentState
        {
            get => _currentState;
            set
            {
                State.text = value.ToString();
                _currentState = value;
            }
        }

        public static event Action<RefillData[]> OnRefill;
        public static event Action OnMatchesCleared;
        public static event Action OnRefillComplete;
        public static event Action OnTilesDropped;

        private void Awake()
        {
            TileSwapper.OnSwapFinished += OnSwapFinished;
            OnRefillComplete += ClearMatches;
            OnMatchesCleared += DropTiles;
            OnTilesDropped += Refill;
        }
        
        private void Start()
        {
            _grid = new Grid(gridSize.x, gridSize.y);
            _matchFinder = new MatchFinder(_grid);
            _grid.PopulateWithRandomTiles();
            CreateLayout();
        }

        public void LogTileState()
        {
            StringBuilder str = new StringBuilder();

            foreach (var views in _tileViews.Where(x=>x.Value == null))
            {
                str.Append(views.Key);
                str.Append(" |");
            }
            str.AppendLine();
            foreach (var mismatchedTiles in _tileViews.Where(tile => tile.Value != null).Where(x => x.Key != x.Value.Tile.GridPosition))
            {
                str.Append("Key: " + mismatchedTiles.Key + " contains tile" + mismatchedTiles.Value.Tile.GridPosition);
            }
            Debug.Log(str.ToString());

        }

        private void OnSwapFinished(TileView tileA, TileView tileB)
        {
            //_tileViews[tileA.Tile.GridPosition] = tileB;
            //_tileViews[tileB.Tile.GridPosition] = tileA;
            //Assert.AreEqual(tileB.Tile.GridPosition, _tileViews[tileA.Tile.GridPosition].Tile.GridPosition);
            //Assert.AreEqual(tileA.Tile.GridPosition, _tileViews[tileB.Tile.GridPosition].Tile.GridPosition);

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
            var aux = tileA;
            _tileViews[tileA.Tile.GridPosition] = tileB;
            _tileViews[tileB.Tile.GridPosition] = aux;

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
            Debug.Log("Cleared Matches");
            LogTileState();
            OnMatchesCleared?.Invoke();
        }

        private void DropTiles()
        {
            var displacedTiles = _grid.DropTiles();
            Debug.Log("Displaced Tiles");
            LogTileState();
            StartCoroutine(DropTiles(displacedTiles));
        }

        private Tile[] GetMatchingTiles()
        {
            return _matchFinder.LookForMatches().SelectMany(x => x.Tiles).Distinct().ToArray();
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
            StringBuilder str = new StringBuilder();
            str.Append("Added tile: ");
            for (var i = 0; i < tilePositions.Length; i++)
            {
                for (var j = 0; j < tilePositions[i].Length; j++)
                {
                    var tilePos = tilePositions[i][j];
                    Debug.Log("TilePos: " + tilePos);
                    var tileView = InstantiateTileView(tilePos.x, tilePos.y, out var rectTransform);
                    _tileViews[tilePos] = tileView;
                    str.Append(tilePos);
                    rectTransform.anchoredPosition = CalculateTilePosition(tilePos.x, -1 - j);
                    refillData[iterator] = new RefillData(rectTransform, CalculateTilePosition(tilePos.x, tilePos.y));
                    iterator++;
                }
            }

            foreach (var tile in _tileViews)
            {
                Assert.IsNotNull(tile.Value, $"Key {tile.Key.ToString()} contains a null value");
            }
            Debug.Log(str.ToString());
            LogTileState();
            StartCoroutine(RefillDrop(refillData));
            Debug.Log("Refilled Tiles");
            OnRefill?.Invoke(refillData);
        }

        public struct RefillData
        {
            public RectTransform Transform;
            public Vector2 FinalPosition;

            public RefillData(RectTransform transform, Vector2 finalPosition)
            {
                Transform = transform;
                FinalPosition = finalPosition;
            }
        }

        private IEnumerator RefillDrop(RefillData[] refillData)
        {
            var time = 0f;
            while (time < timeToFall)
            {
                time += Time.deltaTime;
                var finalPositions = refillData.Select(drop => drop.FinalPosition).ToArray();
                var viewTransforms = refillData.Select(drop => drop.Transform)
                    .ToArray();
                
                var easedTime = Easing.InQuad(time / timeToFall);
                for (var i = 0; i < refillData.Length; i++)
                {
                    viewTransforms[i].anchoredPosition = Vector2.Lerp(viewTransforms[i].anchoredPosition, finalPositions[i], easedTime);
                }
             
                yield return new WaitForEndOfFrame();
            }
            Debug.Log("Refilled Tiles Completed");
            LogTileState();
            OnRefillComplete?.Invoke();
        }

        private IEnumerator DropTiles(Drop[] drops)
        {
            var time = 0f;
            var finalPositions = drops.Select(drop => CalculateTilePosition(drop.NewPosition.x, drop.NewPosition.y)).ToArray();
            var viewTransforms = drops.Select(drop => _tileViews[drop.PreviousPosition])
                .Select(x => x.GetComponent<RectTransform>())
                .ToArray();
            
            for (int i = 0; i < drops.Length; i++)
            {
                _tileViews[drops[i].NewPosition] = _tileViews[drops[i].PreviousPosition];
                _tileViews[drops[i].PreviousPosition] = null;
            }
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

            Debug.Log("Dropped Tiles");
            LogTileState();
            OnTilesDropped?.Invoke();
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