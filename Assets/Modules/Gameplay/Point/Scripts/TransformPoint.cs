using System;
using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace Modules.Gameplay
{
    [Serializable]
    public class TransformPoint<T> : Point<T> where T : class
    {
#if ODIN_INSPECTOR
        [ShowInInspector]
#endif
        private readonly Transform transform;

        public TransformPoint(Transform transform) : base(transform.name)
        {
            this.transform = transform;
        }

        public TransformPoint(string name, Transform transform, T value = null) : base(name, value)
        {
            this.transform = transform;
        }

        public override Vector3 GetPosition()
        {
            return this.transform.position;
        }

        public override Quaternion GetRotation()
        {
            return this.transform.rotation;
        }
    }
}