using System;
using Unity.Burst;
using Unity.Mathematics;
using UnityEngine;

// ReSharper disable ConditionIsAlwaysTrueOrFalse
public static class Mathematics
{
    public static Rect Rect(Vector2 start, Vector2 end)
    {
        var center = (start + end) / 2;
        var size = (end - start).Abs();
        return new Rect(center, size);
    }

    public static Vector2 Abs(this Vector2 vector)
    {
        return new Vector2(Mathf.Abs(vector.x), Mathf.Abs(vector.y));
    }


    /// Возвращает положение точки point относительно линии проходящей через start, end
    public static float PointRelativeToVector(Vector3 start, Vector3 end, Vector3 point)
    {
        return (point.x - start.x) * (end.z - start.z) -
               (point.z - start.z) * (end.x - start.x);
    }

    /// Находит макс растояние target до одной из точкек points
    public static float MaxDistanceToPoint(Vector3[] points, Vector3 target)
    {
        var maxDistance = 0f;
        for (var i = 0; i < points.Length; i++)
        {
            var distance = Vector3.Distance(points[i], target);
            if (distance > maxDistance)
            {
                maxDistance = distance;
            }
        }

        return maxDistance;
    }


    public static float MinDistanceToPoint(Vector3[] points, Vector3 target)
    {
        var minDistance = float.MaxValue;
        for (var i = 0; i < points.Length; i++)
        {
            var distance = Vector3.Distance(points[i], target);
            if (distance < minDistance)
            {
                minDistance = distance;
            }
        }

        return minDistance;
    }

    public static (float, Vector3) MinDistanceToPoint2(Vector3[] points, Vector3 target)
    {
        var minDistance = float.MaxValue;
        var minPoint = Vector3.zero;

        for (var i = 0; i < points.Length; i++)
        {
            var point = points[i];
            var distance = Vector3.Distance(point, target);
            if (distance < minDistance)
            {
                minDistance = distance;
                minPoint = point;
            }
        }

        return (minDistance, minPoint);
    }

    public static float MaxDistanceBetweenPoints(Vector3[] points)
    {
        var count = points.Length;
        if (count < 2)
        {
            return 0.0f;
        }

        var maxDistance = 0f;
        for (var i = 0; i < points.Length; i++)
        {
            for (var j = i + 1; j < count; j++)
            {
                var distance = Vector3.Distance(points[i], points[j]);
                if (distance > maxDistance)
                {
                    maxDistance = distance;
                }
            }
        }

        return maxDistance;
    }

    public static bool RayIntersectsPlane(Ray ray, Plane plane, out float distance)
    {
        return plane.Raycast(ray, out distance);
    }

    public static bool RayIntersectsPlane(Ray ray, Vector3 planePoint, Vector3 planeNormal, out float distance)
    {
        distance = 0f;

        float denom = Vector3.Dot(ray.direction, planeNormal);

        // Если луч и плоскость почти параллельны, то нет пересечения
        if (Mathf.Approximately(denom, 0f))
        {
            return false;
        }

        float t = Vector3.Dot(planePoint - ray.origin, planeNormal) / denom;

        if (t > 0f)
        {
            distance = t;
            return true;
        }

        return false;
    }

    public static Vector3 CenterPoint(Vector3[] points)
    {
        var count = 0;
        Vector3 sum = Vector3.zero;

        foreach (var point in points)
        {
            sum += point;
            count++;
        }

        return sum / count;
    }

    public static bool IsPointOnLine(Vector3 start, Vector3 end, Vector3 point)
    {
        var AB = end - start;
        var AP = point - start;
        var scalar = Vector3.Dot(AP, AB);
        return scalar >= 0 && scalar <= AB.sqrMagnitude;
    }

    public static bool IsPointInRectangle(Vector3 start, Vector3 end, Vector3 point)
    {
        float minX = Mathf.Min(start.x, end.x);
        float maxX = Mathf.Max(start.x, end.x);
        bool checkX;

        if (Mathf.Approximately(minX, maxX))
        {
            checkX = Mathf.Approximately(point.x, minX);
        }
        else
        {
            checkX = point.x >= minX && point.x <= maxX;
        }

        float minZ = Mathf.Min(start.z, end.z);
        float maxZ = Mathf.Max(start.z, end.z);
        bool checkZ;

        if (Mathf.Approximately(minZ, maxZ))
        {
            checkZ = Mathf.Approximately(point.z, minZ);
        }
        else
        {
            checkZ = point.z >= minZ && point.z <= maxZ;
        }

        return checkX && checkZ;
        // return point.x >= minX && point.x <= maxX && point.z >= minZ && point.z <= maxZ;
    }

    public static void RayCircleIntersect(
        Ray ray,
        Vector3 circleCenter,
        float circleRadius,
        out bool result,
        out Vector3 intersect,
        out Vector3 normal
    )
    {
        var s = ray.origin - circleCenter;
        var b = Vector3.Dot(s, ray.direction);
        var c = Vector3.Dot(s, s) - circleRadius * circleRadius;

        var discriminant = b * b - c;

        if (discriminant < 0.0f)
        {
            result = false;
            intersect = default;
            normal = default;
            return;
        }

        var t = -b - MathF.Sqrt(discriminant);
        if (t < 0)
        {
            result = false;
            intersect = default;
            normal = default;
            return;
        }

        intersect = ray.origin + ray.direction * t;
        normal = (intersect - circleCenter).normalized;
        result = true;
    }

    public static bool RayIntoSquareIntersect(
        Ray ray,
        Vector3 squareCenter,
        float squareSize,
        out Vector3 intersect,
        out float distance
    )
    {
        var halfSize = squareSize / 2;
        var topLeft = squareCenter + new Vector3(-halfSize, 0, halfSize);
        var topRight = squareCenter + new Vector3(halfSize, 0, halfSize);
        var bottomLeft = squareCenter + new Vector3(-halfSize, 0, -halfSize);
        var bottomRight = squareCenter + new Vector3(halfSize, 0, -halfSize);

        return RaySegmentIntersect(ray, topLeft, topRight, out intersect, out distance) ||
               RaySegmentIntersect(ray, topLeft, bottomLeft, out intersect, out distance) ||
               RaySegmentIntersect(ray, topRight, bottomRight, out intersect, out distance) ||
               RaySegmentIntersect(ray, bottomLeft, bottomRight, out intersect, out distance);
    }
    
    public static bool RaySegmentIntersect(
        Ray ray,
        Vector3 segmentStart,
        Vector3 segmentEnd,
        out Vector3 intersect,
        out float distance)
    {
        distance = 0f;
        intersect = Vector3.zero;

        Vector3 segmentDirection = segmentEnd - segmentStart;
        Vector3 segmentNormal = new Vector3(-segmentDirection.z, 0, segmentDirection.x).normalized;

        float denom = Vector3.Dot(ray.direction, segmentNormal);

        // Если луч и отрезок почти параллельны, то нет пересечения
        if (Mathf.Approximately(denom, 0f))
        {
            return false;
        }

        float t = Vector3.Dot(segmentStart - ray.origin, segmentNormal) / denom;

        if (t > 0f)
        {
            intersect = ray.origin + ray.direction * t;

            // Проверяем, лежит ли точка пересечения на отрезке
            if (Vector3.Dot(intersect - segmentStart, intersect - segmentEnd) <= 0f)
            {
                distance = t;
                return true;
            }
        }

        return false;
    }

    public static void LineCircleIntersect(
        Vector3 lineStart,
        Vector3 lineEnd,
        Vector3 circleCenter,
        float circleRadius,
        out bool result,
        out Vector3 intersect,
        out Vector3 normal
    )
    {
        var dx = lineEnd.x - lineStart.x;
        var dz = lineEnd.z - lineStart.z;

        var centerX = circleCenter.x;
        var centerZ = circleCenter.z;

        float m;
        float lZ;

        var invert = dx == 0;

        if (invert)
        {
            m = 0;
            lZ = lineStart.x;
        }
        else
        {
            m = dz / dx;
            lZ = lineStart.z - m * lineStart.x;
        }

        // Вычисляем дискриминант:
        float a = 1 + m * m;
        float b = 2 * (m * (lZ - centerZ) - centerX);
        float c = centerX * centerX + (lZ - centerZ) * (lZ - centerZ) - circleRadius * circleRadius;
        float discriminant = b * b - 4 * a * c;

        if (discriminant < 0)
        {
            result = false;
            intersect = default;
            normal = default;
            return;
        }

        // Рассчитываем координаты точек пересечения с прямой: 
        float x1 = (-b + MathF.Sqrt(discriminant)) / (2 * a);
        float z1 = m * x1 + lZ;
        var intersect1 = invert ? new Vector3(z1, 0.0f, x1) : new Vector3(x1, 0.0f, z1);
        var d1 = intersect1 - lineStart;
        var onLine1 = Vector3.Dot(d1, intersect1 - lineEnd) <= 0f;

        float x2 = (-b - MathF.Sqrt(discriminant)) / (2 * a);
        float z2 = m * x2 + lZ;
        var intersect2 = invert ? new Vector3(z2, 0.0f, x2) : new Vector3(x2, 0.0f, z2);
        var d2 = intersect2 - lineStart;
        var onLine2 = Vector3.Dot(d2, intersect2 - lineEnd) <= 0f;

        if (!onLine1 && !onLine2)
        {
            result = false;
            intersect = default;
            normal = default;
            return;
        }

        //Выбираем ближайшую точку
        if (onLine1 && !onLine2)
        {
            intersect = intersect1;
        }
        else if (!onLine1 && onLine2)
        {
            intersect = intersect2;
        }
        else
        {
            if (d1.sqrMagnitude < d2.sqrMagnitude)
            {
                intersect = intersect1;
            }
            else
            {
                intersect = intersect2;
            }
        }

        normal = (intersect - circleCenter).normalized;
        result = true;
    }
}


// public static void LineCylinderIntersect(
//     Vector3 lineStart,
//     Vector3 lineVector,
//     Vector3 cylinderPosition,
//     float cylinderHeight,
//     float cylinderRadius,
//     out bool result,
//     out Vector3 intersection,
//     out Vector3 normal
// )
// {
//     // Vector3 startPoint = ray.origin;
//     // Vector3 direction = ray.direction * distance;
//
//     result = false;
//     intersection = Vector3.zero;
//     normal = Vector3.zero;
//
//     // Calculate cylinder bounds for optimization
//     float cxmin, cymin, czmin, cxmax, cymax, czmax;
//
//     czmin = cylinderPosition.z - cylinderRadius;
//     czmax = cylinderPosition.z + cylinderRadius;
//
//     cymin = cylinderPosition.y - cylinderRadius;
//     cymax = cylinderPosition.y + cylinderRadius;
//
//     cxmin = cylinderPosition.x - cylinderRadius;
//     cxmax = cylinderPosition.x + cylinderRadius;
//
//     // Line out of bounds?
//     if (lineStart.z >= czmax && lineStart.z + lineVector.z > czmax
//         || lineStart.z <= czmin && lineStart.z + lineVector.z < czmin
//         || lineStart.y >= cymax && lineStart.y + lineVector.y > cymax
//         || lineStart.y <= cymin && lineStart.y + lineVector.y < cymin
//         || lineStart.x >= cxmax && lineStart.x + lineVector.x > cxmax
//         || lineStart.x <= cxmin && lineStart.x + lineVector.x < cxmin)
//     {
//         return;
//     }
//
//     Vector3 cylinderEnd = cylinderPosition + Vector3.up * cylinderHeight;
//
//     Vector3 AB = cylinderEnd - cylinderPosition;
//     Vector3 AO = lineStart - cylinderPosition;
//     Vector3 AOxAB = Vector3.Cross(AO, AB);
//     Vector3 VxAB = Vector3.Cross(lineVector, AB);
//
//     float ab2 = Vector3.Dot(AB, AB);
//     float a = Vector3.Dot(VxAB, VxAB);
//     float b = 2 * Vector3.Dot(VxAB, AOxAB);
//     float c = Vector3.Dot(AOxAB, AOxAB) - cylinderRadius * cylinderRadius * ab2;
//
//     float d = b * b - 4f * a * c;
//
//     if (d < 0f)
//     {
//         return;
//     }
//
//     //Root:
//     float time = (-b - Mathf.Sqrt(d)) / (2f * a);
//
//     if (time < 0f)
//     {
//         return;
//     }
//
//     // intersection point
//     intersection = lineStart + lineVector * time;
//
//     // intersection projected onto cylinder axis
//     Vector3 projection = cylinderPosition + Vector3.Dot(AB, intersection - cylinderPosition) / ab2 * AB;
//
//     float test = (projection - cylinderPosition).magnitude + (cylinderEnd - projection).magnitude;
//
//     if (test - 0.01f > AB.magnitude)
//     {
//         return;
//     }
//
//     normal = (intersection - projection).normalized;
//     result = true;
// }

//
// public static class CylinderMath {
//
//     /// <summary>
//     /// Check if a line intersects with a cylinder and return the instersection point
//     /// </summary>
//     /// <param name="startLine">Must be inside the cylinder</param>
//     /// <param name="directionLine">Direction of the line (not the end point)</param>
//     /// <param name="cylinderStart">Center of the bottom circle</param>
//     /// <param name="cylinderEnd">Center of the top circle</param>
//     /// <param name="cylinderRadius">Cylinder radius</param>
//     /// <param name="result">Is there an intersection point?</param>
//     /// <param name="intersection">Intersection point</param>
//     /// <param name="normal">Normal vector</param>
//     public static void LineCylinderIntersectInvert(Vector3 startLine, Vector3 directionLine, Vector3 cylinderStart, Vector3 cylinderEnd, float cylinderRadius, out bool result, out Vector3 intersection, out Vector3 normal) {
//
//         // Invert line
//         Vector3 a = startLine + directionLine;
//         Vector3 b = directionLine * -1f;
//
//         LineCylinderIntersect(a, b, cylinderStart, cylinderEnd, cylinderRadius, out result, out intersection,
//             out normal);
//
//         // Invert normal
//         if (result) {
//             normal *= -1f;
//         }
//
//     }
//
//     
//
// }


// var lineLength = Vector3.Distance(lineStart, lineEnd);
//     
//     //Если длина отрезка равна нулю:
//     if (lineLength <= 0.0f)
// {
//     if (Mathf.Approximately(Vector3.Distance(lineStart, circleCenter), circleRadius))
//     {
//         result = true;
//         intersect = lineStart;
//         normal = (lineStart - circleCenter).normalized;
//     }
//     else
//     {
//         result = false;
//         intersect = default;
//         normal = default;
//     }
//         
//     return;
// }
//
// //Если отрезок внутри окружности:
// if (Vector3.Distance(lineStart, circleCenter) < circleRadius &&
// Vector3.Distance(lineEnd, circleCenter) < circleRadius)
// {
//     result = false;
//     intersect = default;
//     normal = default;
//     return;
// }