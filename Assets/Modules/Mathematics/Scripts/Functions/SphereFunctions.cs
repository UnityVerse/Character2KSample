using Unity.Burst;
using Unity.Mathematics;

namespace Modules.Mathematics
{
    [BurstCompile]
    public static class SphereFunctions
    {
        [BurstCompile]
        public static void IsPointInSphere(in float3 point, in float3 center, in float radius, out bool result)
        {
            result = math.lengthsq(point - center) <= radius * radius;
        }
    }
}


//
// public static Vector3? NearestPointOnCircle(Vector3 P, Vector3 C, float R, Vector3 direction)
// {
//     // Разложим уравнение
//     float a = direction.x * direction.x + direction.z * direction.z;
//     float b = 2 * (direction.x * (P.x - C.x) + direction.y * (P.z - C.z));
//     float c = (P.x - C.x) * (P.x - C.x) + (P.z - C.z) * (P.z - C.z) - R * R;
//
//     // Решим квадратное уравнение at^2 + bt + c = 0
//     float discriminant = b * b - 4 * a * c;
//
//     if (discriminant < 0)
//     {
//         return null; // Нет точек пересечения
//     }
//
//     float sqrtDiscriminant = Mathf.Sqrt(discriminant);
//     float t1 = (-b + sqrtDiscriminant) / (2 * a);
//     float t2 = (-b - sqrtDiscriminant) / (2 * a);
//
//     // Найдем две точки пересечения
//     Vector2 point1 = P + t1 * direction;
//     Vector2 point2 = P + t2 * direction;
//
//     // Выберем ближайшую точку
//     float dist1 = Vector2.Distance(point1, P);
//     float dist2 = Vector2.Distance(point2, P);
//
//     return dist1 < dist2 ? (Vector2?) point1 : point2;
// }