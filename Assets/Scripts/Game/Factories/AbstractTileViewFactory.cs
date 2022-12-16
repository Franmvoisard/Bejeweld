using Shoelace.Bejeweld.Views;
using UnityEngine;

namespace Shoelace.Bejeweld.Factories
{
    public abstract class AbstractTileViewFactory : ScriptableObject
    {
        public abstract TileView CreateTileView(Tile tile);
    }
}