using System.Collections.Generic;
using UnityEngine;

namespace Modules.Gameplay
{
    public static class PointUtils
    {
        public static bool HasFreePoint<T>(this IEnumerable<Point<T>> points) where T : class
        {
            foreach (Point<T> point in points)
            {
                if (point.IsFree())
                {
                    return true;
                }
            }

            return false;
        }
        
        public static bool BindValue<T>(
            this IEnumerable<Point<T>> points,
            in T target,
            in Vector3 position,
            out Point<T> result
        ) where T : class
        {
            result = default;

            if (points.TryGetPoint(target, out result))
            {
                Debug.LogWarning("Target is already bound!");
                return false;
            }

            if (!points.TryGetClosestFreePoint(position, out result))
            {
                return false;
            }

            result.SetValue(target);
            return true;
        }

        public static bool UnbindValue<T>(
            this IEnumerable<Point<T>> points,
            in T target,
            out Point<T> result
        ) where T : class
        {
            if (points.TryGetPoint(target, out result))
            {
                result.ResetValue();
                return true;
            }

            return false;
        }

        public static bool TryGetClosestFreePoint<T>(
            this IEnumerable<Point<T>> points,
            in Vector3 center,
            out Point<T> result
        ) where T : class
        {
            float minDistance = float.MaxValue;
            result = default;

            foreach (Point<T> point in points)
            {
                if (point.IsOccuped())
                {
                    continue;
                }

                Vector3 pPosition = point.GetPosition();
                Vector3 distanceVector = pPosition - center;

                float sqrDistance = distanceVector.sqrMagnitude;
                if (sqrDistance < minDistance)
                {
                    result = point;
                    minDistance = sqrDistance;
                }
            }

            return result != null;
        }

        public static bool TryGetPoint<T>(
            this IEnumerable<Point<T>> points,
            in T target,
            out Point<T> result
        ) where T : class
        {
            foreach (Point<T> point in points)
            {
                if (point.IsValue(target))
                {
                    result = point;
                    return true;
                }
            }

            result = default;
            return false;
        }
    }
}