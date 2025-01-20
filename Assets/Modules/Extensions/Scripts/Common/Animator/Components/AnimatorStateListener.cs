using System;
using UnityEngine;

namespace Atomic.Elements
{
    [DisallowMultipleComponent]
    public sealed class AnimatorStateListener : MonoBehaviour
    {
        public event System.Action<AnimatorStateInfo> OnStateEntered;
        public event System.Action<AnimatorStateInfo> OnStateExited;
    }
}