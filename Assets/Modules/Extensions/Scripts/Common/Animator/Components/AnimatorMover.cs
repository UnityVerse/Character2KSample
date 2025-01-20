using System;
using UnityEngine;

namespace Atomic.Elements
{
    [AddComponentMenu("Modules/Extensions/Animator Mover")]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Animator))]
    public sealed class AnimatorMover : MonoBehaviour
    {
        public event System.Action OnMove;
    
        private void OnAnimatorMove()
        {
            this.OnMove?.Invoke();
        }
    }
}