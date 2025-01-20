using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Mathematics;

namespace Modules.Mathematics
{
    [BurstCompile]
    public static class PolygonFunctions
    {
        public static bool InPolygonXZ(in IReadOnlyList<float3> polygon, in float3 point)
        {
            int n = polygon.Count;
            bool isInside = false;

            for (int i = 0, j = n - 1; i < n; j = i++)
            {
                float3 p1 = polygon[i];
                float3 p2 = polygon[j];
                
                if (p1.z > point.z != p2.z > point.z &&
                    point.x < (p2.x - p1.x) * (point.z - p1.z) / (p2.z - p1.z) + p1.x)
                {
                    isInside = !isInside;
                }
            }

            return isInside;
        }
    }
}




// success = false;
//
// int count = polygon.Length;
// int j = count - 1;
//
// float pointX = point.x;
// float pointZ = point.z;
//
// for (var i = 0; i < count; i++)
// {
//     float3 cPoint = polygon[i];
//     float cPointX = cPoint.x;
//     float cPointZ = cPoint.z;
//
//     float3 oPoint = polygon[j];
//     float oPointX = oPoint.x;
//     float oPointZ = oPoint.z;
//
//     if (cPointZ < pointZ && oPointZ >= pointZ || oPointZ < pointZ && cPointZ >= pointZ)
//     {
//         if (cPointX + (pointZ - cPointZ) / (oPointZ - cPointZ) * (oPointX - cPointX) < pointX)
//         {
//             success = !success;
//         }
//     }
//
//     j = i;