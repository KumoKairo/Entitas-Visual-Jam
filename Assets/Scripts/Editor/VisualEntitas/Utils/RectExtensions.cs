using UnityEngine;

namespace Entitas.Visual.Utils
{
    public static class RectExtensions
    {
        public static Rect Offset(this Rect rect, Rect offset)
        {
            return new Rect(rect.x + offset.x, rect.y + offset.y, rect.width + offset.width, rect.height + offset.height);
        }
    }
}