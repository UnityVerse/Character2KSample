using System;
using Atomic.AI;
using Unity.Mathematics;
using UnityEngine;

namespace Modules.Extensions
{
    [Serializable]
    public sealed class OverlapSphereNonAllocBlackboardFunction : IColliderBufferBlackboardAction
    {
        [BlackboardKey]
        [SerializeField]
        private int centerKey;

        [BlackboardKey]
        [SerializeField]
        private int radiusKey;

        [SerializeField]
        private LayerMask layerMask;

        [SerializeField]
        private QueryTriggerInteraction queryTriggerInteraction;

        public void Invoke(IBlackboard blackboard, ref BufferData<Collider> buffer)
        {
            float3 position = blackboard.GetObject<Transform>(this.centerKey).position;
            float radius = blackboard.GetFloat(this.radiusKey);
            
            buffer.size =  Physics.OverlapSphereNonAlloc(
                position,
                radius,
                buffer.values,
                this.layerMask,
                this.queryTriggerInteraction
            );
        }
    }
}