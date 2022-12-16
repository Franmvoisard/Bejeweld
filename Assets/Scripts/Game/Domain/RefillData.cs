using UnityEngine;

namespace Shoelace.Bejeweld
{
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
}