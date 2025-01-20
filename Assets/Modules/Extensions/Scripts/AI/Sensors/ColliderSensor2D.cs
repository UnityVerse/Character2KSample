using System;
using Atomic.AI;
using UnityEngine;

namespace Modules.Extensions
{
    [Serializable]
    public sealed class ColliderSensor2D : IUpdateAIBehaviour
    {
        [BlackboardKey]
        [SerializeField]
        private int center;

        [BlackboardKey]
        [SerializeField]
        private int radius;

        [SerializeField]
        private LayerMask layerMask;

        [BlackboardKey]
        [SerializeField]
        private int collidersBuffer;

        public void OnUpdate(IBlackboard blackboard, float deltaTime)
        {
            Transform transform = blackboard.GetObject<Transform>(this.center);
            float radius = blackboard.GetFloat(this.radius);

            ref var buffer = ref blackboard.GetStruct<BufferData<Collider2D>>(this.collidersBuffer);

            int size = Physics2D.OverlapCircleNonAlloc(
                transform.position,
                radius,
                buffer.values,
                this.layerMask
            );

            buffer.size = size;
        }
    }
}