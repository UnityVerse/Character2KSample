// using Unity.Burst;
// using Unity.Mathematics;
//
// namespace Modules.Extensions
// {
//     [BurstCompile]
//     public static class MathUtils
//     {
//         [BurstCompile]
//         public static float3 SmoothDamp(
//             float3 current,
//             float3 target,
//             ref float3 currentVelocity,
//             float smoothTime,
//             float maxSpeed,
//             float deltaTime
//         )
//         {
//             // smoothTime is the time to reach the target (in seconds) when at max speed
//             smoothTime = math.max(0.0001f, smoothTime); // ensure smoothTime is positive and not zero
//
//             float3 currentToTarget = target - current;
//             float maxLength = maxSpeed * smoothTime;
//             float3 clampedDelta = math.clamp(currentToTarget, -maxLength, maxLength);
//             float3 targetVelocity = 2.0f / smoothTime * clampedDelta;
//
//             float3 result = current + currentVelocity * deltaTime;
//             float3 newVelocity = currentVelocity + targetVelocity * deltaTime;
//
//             currentVelocity = newVelocity;
//             return result;
//         }
//     }
// }