using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AudioEngine
{
    [Serializable]
    [InlineProperty]
    public struct AudioParameterKey : ISerializationCallbackReceiver
    {
        [HorizontalGroup]
        [OnValueChanged("OnValidate")]
        [SerializeField]
        private AudioBank bank;

        [HorizontalGroup]
        [LabelText("Param")]
        [ValueDropdown("ValuesDropdown")]
        [OnValueChanged("OnValidate")]
        [SerializeField]
        private string parameterId;  //Выдавать список параметров dropdown

        [SerializeField, HideInInspector]
        private string identifier; 

        public static implicit operator string(AudioParameterKey it)
        {
            return it.identifier;
        }
        
        private void OnValidate()
        {
            if (this.bank != null && this.parameterId != null)
            {
                this.identifier = this.bank.identifier + "." + this.parameterId;
            }
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            if (this.bank != null)
            {
                this.identifier = this.bank.identifier + "." + this.parameterId;
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
            
            return bank.GetAllParameterIds();
        }
#endif
    }
}