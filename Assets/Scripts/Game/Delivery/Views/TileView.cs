using System;
using Shoelace.Bejeweld.Components;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Shoelace.Bejeweld.Views
{
    public class TileView : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler
    {
        public Image ImageComponent { get; private set; }

        private void Awake()
        {
            ImageComponent = GetComponent<Image>();
        }

        public Tile Tile { get; private set; }
        
        public void SetTile(Tile tile)
        {
            Tile = tile;
            gameObject.name = tile.GridPosition.ToString();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            TileSelector.Select(this, SelectionType.Click);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if(Input.GetMouseButton(0))
            {
                TileSelector.Select(this, SelectionType.Drag);
            }
        }
    }

    public enum SelectionType
    {
        Click,
        Drag,
        EndDrag
    }
}