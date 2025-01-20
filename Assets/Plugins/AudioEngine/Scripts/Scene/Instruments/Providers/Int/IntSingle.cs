using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AudioEngine
{
    [Serializable, InlineProperty]
    public sealed class IntSingle : IIntProvider
    {
        [SerializeField]
        private int value = 1;
        
        public int Value => this.value;
    }
}