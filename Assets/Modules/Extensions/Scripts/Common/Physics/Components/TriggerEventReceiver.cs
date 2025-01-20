using System;
using System.Collections.Generic;
using Modules.Extensions;
using UnityEngine;
using UnityEngine.Events;

namespace Atomic.Elements
{
    [AddComponentMenu("Modules/Extensions/Trigger Event Receiver")]
    [DisallowMultipleComponent]
    public class TriggerEventReceiver : MonoBehaviour
    {
        public event System.Action<Collider> OnEntered; 
        public event System.Action<Collider> OnExited;
        public event System.Action<Collider> OnStay;

        [Header("Collider Conditions")]
        [SerializeReference]
        public List<IColliderCondition> enterConditions = new();

        [SerializeReference]
        public List<IColliderCondition> stayConditions = new();
        
        [SerializeReference]
        public List<IColliderCondition> exitConditions = new();
        
        [Header("Collider Actions")]
        [SerializeReference]
        public List<IColliderAction> enterActions = new();

        [SerializeReference]
        public List<IColliderAction> exitActions = new();
        
        [SerializeReference]
        public List<IColliderAction> stayActions = new();

        [Header("Base Actions")]
        [SerializeReference]
        public List<IAction> enterBaseActions = new();
        
        [SerializeReference]
        public List<IAction> exitBaseActions = new();

        [SerializeReference]
        public List<IAction> stayBaseActions = new();

        [Header("Unity Events")]
        [SerializeField]
        public UnityEvent<Collider> enterEvent;
        
        [SerializeField]
        public UnityEvent<Collider> stayEvent;
        
        [SerializeField]
        public UnityEvent<Collider> exitEvent;

        private void OnTriggerEnter(Collider collider)
        {
            if (this.enterConditions != null)
            {
                for (int i = 0, count = this.enterConditions.Count; i < count; i++)
                {
                    IColliderCondition condition = this.enterConditions[i];
                    if (condition != null && !condition.Invoke(collider))
                    {
                        return;
                    }
                }    
            }

            if (this.enterActions != null)
            {
                for (int i = 0, count = this.enterActions.Count; i < count; i++)
                {
                    IColliderAction action = this.enterActions[i];
                    action?.Invoke(collider);
                }    
            }

            if (this.enterBaseActions != null)
            {
                for (int i = 0, count = this.enterBaseActions.Count; i < count; i++)
                {
                    IAction action = this.enterBaseActions[i];
                    action?.Invoke();
                }  
            }

            this.enterEvent.Invoke(collider);
            this.OnEntered?.Invoke(collider);
        }

        private void OnTriggerStay(Collider collider)
        {
            if (this.stayConditions != null)
            {
                for (int i = 0, count = this.stayConditions.Count; i < count; i++)
                {
                    IColliderCondition condition = this.stayConditions[i];
                    if (condition != null && !condition.Invoke(collider))
                    {
                        return;
                    }
                }
            }
            
            if (this.stayActions != null)
            {
                for (int i = 0, count = this.stayActions.Count; i < count; i++)
                {
                    IColliderAction action = this.stayActions[i];
                    action?.Invoke(collider);
                }
            }

            if (this.stayBaseActions != null)
            {
                for (int i = 0, count = this.stayBaseActions.Count; i < count; i++)
                {
                    IAction action = this.stayBaseActions[i];
                    action?.Invoke();
                }
            }
            
            this.stayEvent.Invoke(collider);
            this.OnStay?.Invoke(collider);
        }

        private void OnTriggerExit(Collider collider)
        {
            if (this.exitConditions != null)
            {
                for (int i = 0, count = this.exitConditions.Count; i < count; i++)
                {
                    IColliderCondition condition = this.exitConditions[i];
                    if (condition != null && !condition.Invoke(collider))
                    {
                        return;
                    }
                }
            }
            
            if (this.exitActions != null)
            {
                for (int i = 0, count = this.exitActions.Count; i < count; i++)
                {
                    IColliderAction action = this.exitActions[i];
                    action?.Invoke(collider);
                }
            }
            
            if (this.exitBaseActions != null)
            {
                for (int i = 0, count = this.exitBaseActions.Count; i < count; i++)
                {
                    IAction action = this.exitBaseActions[i];
                    action?.Invoke();
                }
            }
            
            this.exitEvent.Invoke(collider);
            this.OnExited?.Invoke(collider);
        }
    }
}