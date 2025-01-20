using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Atomic.Elements
{
    [AddComponentMenu("Modules/Extensions/Animation Event Receiver")]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Animator))]
    public sealed class AnimationEventListener : MonoBehaviour, ISerializationCallbackReceiver
    {
        public event System.Action<string> OnMessageReceived;

        [SerializeField]
        [FormerlySerializedAs("events")]
        private Reaction[] reactions;

        private Dictionary<string, List<System.Action>> eventBus;

        public void Subscribe(string eventName, System.Action reaction)
        {
            if (string.IsNullOrEmpty(eventName) || reaction == null)
            {
                return;
            }
            
            if (!this.eventBus.TryGetValue(eventName, out List<System.Action> reactions))
            {
                reactions = new List<System.Action>();
                this.eventBus.Add(eventName, reactions);
            }
            
            reactions.Add(reaction);
        }

        public void Unsubscribe(string eventName, System.Action reaction)
        {
            if (string.IsNullOrEmpty(eventName) || reaction == null)
            {
                return;
            }
            
            if (this.eventBus.TryGetValue(eventName, out List<System.Action> reactions))
            {
                reactions.Remove(reaction);
            }
        }

        [UsedImplicitly]
        internal void ReceiveEvent(string message)
        {
            if (this.eventBus.TryGetValue(message, out List<System.Action> reactions))
            {
                for (int i = 0, count = reactions.Count; i < count; i++)
                {
                    System.Action reaction = reactions[i];
                    reaction.Invoke();
                }
            }

            this.OnMessageReceived?.Invoke(message);
        }


        public void OnAfterDeserialize()
        {
            this.eventBus = new Dictionary<string, List<System.Action>>();

            for (int i = 0, count = this.reactions.Length; i < count; i++)
            {
                Reaction reaction = this.reactions[i];
                this.Subscribe(reaction.message, reaction.action.Invoke);
            }
        }

        public void OnBeforeSerialize()
        {
        }

        [Serializable]
        private struct Reaction
        {
            [SerializeField]
            internal string message;

            [SerializeField]
            internal UnityEvent action;
        }
    }
}