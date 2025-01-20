using System;
using UnityEngine;

namespace Atomic.AI
{
    [Serializable]
    public sealed class Ref<T> : IRef
    {
        object IRef.Value => value;

        [SerializeField]
        public T value;

        internal Ref(T value)
        {
            this.value = value;
        }

        public static implicit operator T(Ref<T> it)
        {
            return it.value;
        }
    }
}