using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
// ReSharper disable NotAccessedField.Local

namespace Atomic.AI
{
    [MovedFrom(true, "Modules.AI", "Modules.AI.BehaviourSet", "CompositeBehaviour")]
    [Serializable]
    public sealed class CompositeAIBehaviour :
        IStartAIBehaviour,
        IStopAIBehaviour,
        IUpdateAIBehaviour,
        ISerializationCallbackReceiver
    {
        #if UNITY_EDITOR

        [GUIColor(1f, 0.92156863f, 0.015686275f)]
        [SerializeField, HideLabel]
        private string name;
        
        #endif
        
        [SerializeReference]
        private List<IAIBehaviour> behaviours = default;

        private List<IUpdateAIBehaviour> updateBehaviours = new();

        public void OnStart(IBlackboard blackboard)
        {
            if (this.behaviours != null)
            {
                for (int i = 0, count = this.behaviours.Count; i < count; i++)
                {
                    if (this.behaviours[i] is IStartAIBehaviour behaviour)
                    {
                        behaviour.OnStart(blackboard);
                    }
                }
            }
        }

        public void OnStop(IBlackboard blackboard)
        {
            if (this.behaviours != null)
            {
                for (int i = 0, count = this.behaviours.Count; i < count; i++)
                {
                    if (this.behaviours[i] is IStopAIBehaviour behaviour)
                    {
                        behaviour.OnStop(blackboard);
                    }
                }
            }
        }

        public void OnUpdate(IBlackboard blackboard, float deltaTime)
        {
            if (this.updateBehaviours.Count == 0)
            {
                return;
            }

            for (int i = 0, count = this.updateBehaviours.Count; i < count; i++)
            {
                IUpdateAIBehaviour logic = this.updateBehaviours[i];
                logic.OnUpdate(blackboard, deltaTime);
            }
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            this.updateBehaviours = new List<IUpdateAIBehaviour>();
            
            if (this.behaviours == null)
            {
                return;
            }
            
            for (int i = 0, count = this.behaviours.Count; i < count; i++)
            {
                if (this.behaviours[i] is IUpdateAIBehaviour updateBehaviour)
                {
                    this.updateBehaviours.Add(updateBehaviour);
                }
            }
        }

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
        }
    }
}