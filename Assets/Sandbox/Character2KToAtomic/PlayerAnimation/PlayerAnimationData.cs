using System;
using UnityEngine;

namespace Sandbox
{
    [Serializable]
    public sealed class PlayerAnimationData
    {
        public GameObject gameObject;
        public Transform transform;
        
        // fields for all player components to avoid costly GetComponent calls

        [Header("Animation")]
        public float animationDirectionDampening = 0.05f;
        public float animationTurnDampening = 0.1f;
        public Vector3 lastForward;
    }
}