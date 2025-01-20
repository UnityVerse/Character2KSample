using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AudioEngine
{
    [Serializable, InlineProperty]
    internal sealed class FloatSingle : IFloatProvider
    {
        [SerializeField]
        private float value = 1;

        public FloatSingle()
        {
        }

        public FloatSingle(float value)
        {
            this.value = value;
        }

        public float Value => this.value;
    }
}