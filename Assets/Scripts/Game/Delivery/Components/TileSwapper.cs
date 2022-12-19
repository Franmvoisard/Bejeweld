using System;
using System.Collections;
using Shoelace.Bejeweld.Views;
using UnityEngine;

namespace Shoelace.Bejeweld.Components
{
    public class TileSwapper : MonoBehaviour
    {
        [SerializeField] private GridView gridView;
        [SerializeField] private float timeToSwap = 0.25f;
        
        public static event Action<TileView,TileView> OnSwapFinished;

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
                StartCoroutine(DoSwap(TileSelector.SelectedTile, tileView));
            }
        }

        private IEnumerator DoSwap(TileView tileA, TileView tileB)
        {
            gridView.Swap(tileA.Tile, tileB.Tile);
            var auxTile = tileA.Tile;
            tileA.SetTile(tileB.Tile);
            tileB.SetTile(auxTile);
            TileSelector.SelectedTile = null;
            var tileAStartPosition = tileA.transform.position;
            var tileBStartPosition = tileB.transform.position;
            float time = 0;
            
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