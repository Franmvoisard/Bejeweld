using Shoelace.Bejeweld.Factories;
using UnityEngine;

namespace Shoelace.Bejeweld.Components
{
    public class Board : MonoBehaviour
    {
        [Header("Grid Settings")]
        [SerializeField] private Transform tileParent;
        [SerializeField] private Vector2Int gridSize;
        [SerializeField] private int tileSize = 100;
        [SerializeField] private int tileSpacing = 100;
        
        [Header("Tile Settings")]
        [SerializeField] private AbstractTileViewFactory tileViewFactory;
        private IGrid _grid;
        private void Start()
        {
            _grid = new Grid(gridSize.x, gridSize.y);
            _grid.PopulateWithRandomTiles();
            CreateLayout();
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
                }
            }
        }

        private Vector2 CalculateTilePosition(int row, int column)
        {
            return new Vector2(row * tileSize + tileSpacing * row, column * -tileSize - tileSpacing * column);
        }
    }
}