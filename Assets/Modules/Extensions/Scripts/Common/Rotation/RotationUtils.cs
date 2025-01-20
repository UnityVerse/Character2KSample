using Unity.Burst;
using Unity.Mathematics;

namespace Modules.Extensions
{
    [BurstCompile]
    public static class RotationUtils
    {
        [BurstCompile]
        public static void RotateTowards(
            in quaternion currentRotation,
            in float3 direction,
            in float deltaTime,
            in float speed,
            out quaternion result
        )
        {
            quaternion targetRotation = quaternion.LookRotation(direction, new float3(0, 1, 0));
            float percent = speed * deltaTime;
            result = math.slerp(currentRotation, targetRotation, percent);
        }
    }
}