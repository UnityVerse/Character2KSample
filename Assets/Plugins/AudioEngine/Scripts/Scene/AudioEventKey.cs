using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AudioEngine
{
    [Serializable, InlineProperty]
    public struct AudioEventKey : ISerializationCallbackReceiver
    {
        [SerializeField]
        [OnValueChanged("OnValidate")]
        private AudioBank bank;

        [ValueDropdown("ValuesDropdown")]
        [LabelText("Event")]
        [OnValueChanged("OnValidate")]
        [SerializeField]
        private string eventId; //Выдавать список ивентов dropdown

        [SerializeField, HideInInspector]
        private string identifier;

        public static implicit operator string(AudioEventKey it)
        {
            return it.identifier;
        }

        private void OnValidate()
        {
            if (this.bank != null && this.eventId != null)
            {
                this.identifier = this.bank.identifier + "." + this.eventId;
            }
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            this.OnValidate();
        }

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
        }

#if UNITY_EDITOR
        private IEnumerable<string> ValuesDropdown()
        {
            if (bank == null)
            {
                return new List<string>();
            }

            return this.bank.GetEventIds();
        }
#endif
    }
}