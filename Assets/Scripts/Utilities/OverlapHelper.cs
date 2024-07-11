using System.Collections.Generic;
using UnityEngine;

namespace Utilities
{
    public static class OverlapHelper
    {
        public static List<T> GetComponentsInCircle<T>(Vector2 center, float range, LayerMask layerMask)
        {
            var results = new List<T>();
            var colliders = Physics2D.OverlapCircleAll(center, range, layerMask);
            
            foreach (var item in colliders)
            {
                if (item.TryGetComponent<T>(out var target))
                    results.Add(target);
            }

            return results;
        }
    }
}