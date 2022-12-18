using Shoelace.Bejeweld.Views;

namespace Shoelace.Bejeweld.Components
{
    public delegate void TileSelected(TileView tile, SelectionType type);

    public static class TileSelector
    {
        public static TileView SelectedTile;
        public static event TileSelected OnTileSelected;

        public static void Select(TileView tileView, SelectionType selectionType)
        {
            OnTileSelected?.Invoke(tileView, selectionType);
        }
    }
}