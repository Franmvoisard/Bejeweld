using System;
using UnityEngine;

namespace Shoelace.Bejeweld.Views
{
    [Serializable]
    public struct TileViewSetting
    {
        public TileView prefab;
        public Color color;
        public Sprite sprite;
        public int tileType;
    }
}