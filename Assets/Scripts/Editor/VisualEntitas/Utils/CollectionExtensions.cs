using System.Collections.Generic;

namespace Entitas.Visual.Utils
{
    public static class CollectionExtensions
    {
        public static void AddOnce<T>(this List<T> list, T item)
        {
            if (!list.Contains(item))
            {
                list.Add(item);
            }
        }
    }
}