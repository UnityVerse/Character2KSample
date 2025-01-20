// using System;
// using Unity.Mathematics;
//
// namespace Modules.Extensions
// {
//     public sealed class Transform : ITransform
//     {
//         private readonly Func<float3> positionFunc;
//         private readonly Func<quaternion> rotationFunc;
//
//         public float3 GetPosition()
//         {
//             return this.positionFunc.Invoke();
//         }
//
//         public quaternion GetRotation()
//         {
//             return this.rotationFunc.Invoke();
//         }
//     }
// }