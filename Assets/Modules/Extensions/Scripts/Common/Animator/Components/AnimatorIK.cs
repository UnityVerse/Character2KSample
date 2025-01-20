using System;
using UnityEngine;

namespace Atomic.Elements
{
    [AddComponentMenu("Modules/Extensions/Animator IK")]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Animator))]
    public sealed class AnimatorIK : MonoBehaviour
    {
        public event System.Action OnIK;
    
        private void OnAnimatorIK(int layerIndex)
        {
            this.OnIK?.Invoke();
        }
    }
}