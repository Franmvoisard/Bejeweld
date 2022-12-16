using Shoelace.Bejeweld.Views;

namespace Shoelace.Bejeweld.Components
{
    public delegate void TileSelected(TileView tile, SelectionType type);

    public static class TileSelector
    {
        public static TileView SelectedTile;
        private static bool _isEnabled = true;
        public static event TileSelected OnTileSelected;

        public static void Select(TileView tileView, SelectionType selectionType)
        {
            if (_isEnabled == false) return;
            OnTileSelected?.Invoke(tileView, selectionType);
        }

        public static void EmptySelection() => SelectedTile = null;

        public static void Disable() => _isEnabled = false;

        public static void EnableSelection() => _isEnabled = true;
    }
}