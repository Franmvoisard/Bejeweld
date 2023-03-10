using System.Collections.Generic;
using Shoelace.Bejeweld.Views;
using UnityEngine;
using UnityEngine.UI;

namespace Shoelace.Bejeweld.Factories
{
    [CreateAssetMenu(menuName = "Create TileFactory", fileName = "TileFactory", order = 0)]
    public class TileFactory : AbstractTileViewFactory
    {
        [SerializeField]
        private List<TileViewSetting> tileViewSettings;
        public override TileView CreateTileView(Tile tile)
        {
            var settings = tileViewSettings[tile.TypeId];
            var tileView = Object.Instantiate(settings.prefab);
            var image = tileView.GetComponent<Image>();
            image.color = settings.color;
            image.sprite = settings.sprite;
            tileView.SetTile(tile);
            return tileView;
        }
    }
}