using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AudioEngine
{
    [Serializable, InlineProperty]
    internal struct BoolParameter
    {
        [HorizontalGroup]
        [SerializeField]
        internal string identifier;

        [HorizontalGroup]
        [SerializeField]
        internal bool value;
    }
}