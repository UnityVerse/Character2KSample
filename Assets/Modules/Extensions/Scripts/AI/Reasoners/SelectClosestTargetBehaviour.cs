using System;
using Atomic.AI;
using Atomic.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Modules.Extensions
{
    [Serializable]
    public sealed class SelectClosestTargetBehaviour : IUpdateAIBehaviour
    {
        [BlackboardKey]
        [SerializeField]
        private int bufferKey;

        [BlackboardKey]
        [SerializeField]
        private int centerKey;

        [BlackboardKey]
        [SerializeField]
        private int targetKey;

        public void OnUpdate(IBlackboard blackboard, float deltaTime)
        {
            ref var buffer = ref blackboard.GetStruct<BufferData<Collider>>(this.bufferKey);

            if (this.SelectClosestTarget(blackboard, buffer, out IEntity target))
            {
                blackboard.SetObject(this.targetKey, target);
            }
            else
            {
                blackboard.DelObject(this.targetKey);
            }
        }

        private bool SelectClosestTarget(
            IBlackboard blackboard,
            BufferData<Collider> buffer,
            out IEntity target
        )
        {
            target = null;

            int count = buffer.size;
            if (count == 0)
            {
                return false;
            }

            float3 pivotPosition = blackboard.GetObject<Transform>(this.centerKey).position;
            float minDistance = float.MaxValue;

            Collider[] colliders = buffer.values;

            for (int i = 0; i < count; i++)
            {
                Collider collider = colliders[i];
                if (!collider.TryGetEntity(out IEntity other))
                {
                    continue;
                }

                float3 otherPosition = collider.transform.position;
                float distanceSq = math.lengthsq(otherPosition - pivotPosition);

                if (distanceSq <= minDistance)
                {
                    minDistance = distanceSq;
                    target = other;
                }
            }

            return target != null;
        }
    }
}


// private bool ContainsInBuffer(IObject target, Collider[] buffer, int bufferSize)
// {
//     for (int i = 0; i < bufferSize; i++)
//     {
//         Collider collider = buffer[i];
//         if (!collider.TryGetSceneObject(out IObject other))
//         {
//             continue;
//         }
//         
//         if (target.Id == other.Id)
//         {
//             return true;
//         }
//     }
//
//     return false;
// }


// private GameObject FindTarget()
// {
//     GameObject target = null;
//
//     int count = this.ScanColliders(buffer);
//
//     Vector3 pivotPosition = pivot.transform.position;
//     float minDistance = float.MaxValue;
//
//     for (int i = 0; i < count; i++)
//     {
//         Collider collider = buffer[i];
//
//         float distance = (collider.transform.position - pivotPosition).sqrMagnitude;
//         if (distance <= minDistance)
//         {
//             minDistance = distance;
//             target = collider.gameObject;
//         }
//     }
//
//     return target;
// }