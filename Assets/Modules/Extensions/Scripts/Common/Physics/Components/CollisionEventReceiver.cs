using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Atomic.Elements
{
    [AddComponentMenu("Modules/Extensions/Collision Event Receiver")]
    [DisallowMultipleComponent]
    public sealed class CollisionEventReceiver : MonoBehaviour
    {
        public event System.Action<Collision> OnEntered; 
        public event System.Action<Collision> OnExited; 
        public event System.Action<Collision> OnStay;

        [Header("Actions")]
        [SerializeReference]
        public List<ICollisionAction> enterActions = new();

        [SerializeReference]
        public List<ICollisionAction> exitActions = new();
        
        [SerializeReference]
        public List<ICollisionAction> stayActions = new();

        [Header("Events")]
        public UnityEvent enterEvent;

        private void OnCollisionEnter(Collision collision)
        {
            for (int i = 0, count = this.enterActions.Count; i < count; i++)
            {
                ICollisionAction action = this.enterActions[i];
                action?.Invoke(collision);
            }
            
            this.OnEntered?.Invoke(collision);
        }

        private void OnCollisionStay(Collision collision)
        {
            if (this.stayActions != null)
            {
                for (int i = 0, count = this.stayActions.Count; i < count; i++)
                {
                    ICollisionAction action = this.stayActions[i];
                    action?.Invoke(collision);
                }
            }
            
            this.OnStay?.Invoke(collision);
        }

        private void OnCollisionExit(Collision collision)
        {
            if (this.exitActions != null)
            {
                for (int i = 0, count = this.exitActions.Count; i < count; i++)
                {
                    ICollisionAction action = this.exitActions[i];
                    action?.Invoke(collision);
                }
            }
            
            this.OnExited?.Invoke(collision);
        }
    }
}