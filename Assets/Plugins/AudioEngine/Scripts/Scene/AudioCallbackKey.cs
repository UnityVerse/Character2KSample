using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AudioEngine
{
    [Serializable, InlineProperty]
    public struct AudioCallbackKey : ISerializationCallbackReceiver
    {
        [HorizontalGroup]
        [SerializeField]
        [OnValueChanged("OnValidate")]
        private AudioBank bank;

        [HorizontalGroup]
        [ValueDropdown("ValuesDropdown")]
        [OnValueChanged("OnValidate")]
        [LabelText("Callback")]
        [SerializeField]
        private string callbackId; //Выдавать список ивентов dropdown

        [SerializeField, HideInInspector]
        private string identifier;

        public static implicit operator string(AudioCallbackKey it)
        {
            return it.identifier;
        }
        
        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
           this.OnValidate();
        }
        
        
        private void OnValidate()
        {
            if (this.bank != null && this.callbackId != null)
            {
                this.identifier = this.bank.identifier + "." + this.callbackId;
            }
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
            
            return this.bank.GetCallbacks();
        }
#endif
    }
}