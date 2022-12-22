using System;
using System.Collections;
using System.Linq;
using Shoelace.Bejeweld.Views;
using UnityEngine;

namespace Shoelace.Bejeweld.Components
{
    public class TileSwapper : MonoBehaviour
    {
        [SerializeField] private GridView gridView;
        [SerializeField] private float timeToSwap = 0.25f;
        
        public static event Action<TileView,TileView> OnSwapFinished;
        public static event Action OnSwapFailed;

        private void Awake()
        {
            TileSelector.OnTileSelected += OnTileSelected;
        }

        private void OnTileSelected(TileView tileView, SelectionType type)
        {
            if (gridView.CurrentState != GridState.Interactable) return;
            SelectTileOrSwap(tileView, type);
        }

        private void SelectTileOrSwap(TileView tileView, SelectionType type)
        {
            if (TileSelector.SelectedTile == null)
            {
                TileSelector.SelectedTile = tileView;
            }
            else
            {
                if (TileSelector.SelectedTile == tileView) return;
                if (gridView.IsAdjacent(tileView, TileSelector.SelectedTile) == false)
                {
                    TileSelector.SelectedTile = tileView;
                    return;
                }
                StartCoroutine(DoSwap(TileSelector.SelectedTile, tileView));
            }
        }

        private void OnDestroy()
        {
            TileSelector.OnTileSelected -= OnTileSelected;
        }

        private IEnumerator DoSwap(TileView tileA, TileView tileB)
        {
            gridView.Swap(tileA, tileB);
            float time = 0;
            var tileAStartPosition = tileA.transform.position;
            var tileBStartPosition = tileB.transform.position;

            if (gridView.MatchFinder.LookForMatches().Any() == false)
            {
                gridView.Swap(tileA, tileB);
                while (time < timeToSwap)
                {
                    time += Time.deltaTime;
                    tileA.transform.position =
                        Vector3.Lerp(tileAStartPosition, tileBStartPosition, time / timeToSwap);
                    tileB.transform.position =
                        Vector3.Lerp(tileBStartPosition, tileAStartPosition, time / timeToSwap);
                    yield return null;
                }
                time = 0f;
                while (time < timeToSwap)
                {
                    time += Time.deltaTime;
                    tileA.transform.position =
                        Vector3.Lerp(tileBStartPosition, tileAStartPosition, time  / timeToSwap);
                    tileB.transform.position =
                        Vector3.Lerp(tileAStartPosition, tileBStartPosition, time  / timeToSwap);
                    yield return null;
                }

                OnSwapFailed?.Invoke();
                TileSelector.EmptySelection();
                yield break;
            }

            TileSelector.EmptySelection();
            
            while (time < timeToSwap)
            {
                time += Time.deltaTime;
                tileA.transform.position = Vector3.Lerp(tileAStartPosition, tileBStartPosition, time / timeToSwap);
                tileB.transform.position = Vector3.Lerp(tileBStartPosition, tileAStartPosition, time / timeToSwap);
                yield return null;
            }

            OnSwapFinished?.Invoke(tileA, tileB);
        }
    }
}