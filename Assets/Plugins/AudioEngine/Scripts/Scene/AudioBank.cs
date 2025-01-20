using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace AudioEngine
{
    [CreateAssetMenu(
        fileName = "AudioBank",
        menuName = "AudioEngine/Scene/New AudioBank"
    )]
    public sealed class AudioBank : ScriptableObject
    {
        [SerializeField]
        internal string identifier;

        [SerializeField]
        internal EventParameter[] eventParameters;

        [Space(8)]
        [SerializeField]
        internal BoolParameter[] boolParameters;

        [Space(8)]
        [SerializeField]
        internal IntParameter[] intParameters;

        [Space(8)]
        [SerializeField]
        internal FloatParameter[] floatParameters;

        [Space(8)]
        [SerializeField]
        internal string[] callbacks;

        public string[] GetCallbacks()
        {
            return this.callbacks;
        }
        
        public string GetIdentifier()
        {
            return this.identifier;
        }

        public IEnumerable<string> GetEventIds()
        {
            return this.eventParameters.Select(it => it.identifier);
        }

        public IEnumerable<string> GetAllParameterIds()
        {
            foreach (BoolParameter parameter in this.boolParameters)
            {
                yield return parameter.identifier;
            }

            foreach (IntParameter parameter in this.intParameters)
            {
                yield return parameter.identifier;
            }

            foreach (FloatParameter parameter in this.floatParameters)
            {
                yield return parameter.identifier;
            }
        }
    }
}