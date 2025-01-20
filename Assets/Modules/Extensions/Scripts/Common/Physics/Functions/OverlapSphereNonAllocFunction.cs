using System;
using Atomic.Elements;
using UnityEngine;

namespace Modules.Extensions
{
    [Serializable]
    public sealed class OverlapSphereNonAllocFunction : IColliderBufferFunction
    {
        [SerializeField]
        private Transform center;

        [SerializeField]
        private float radius;

        [SerializeField]
        private LayerMask layerMask;

        [SerializeField]
        private QueryTriggerInteraction queryTriggerInteraction;

        public int Invoke(Collider[] buffer)
        {
            return Physics.OverlapSphereNonAlloc(
                this.center.position,
                this.radius,
                buffer,
                this.layerMask,
                this.queryTriggerInteraction
            );
        }
    }
}