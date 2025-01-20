using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Atomic.AI
{
    [Serializable]
    public abstract class BehaviourNodeAsset<TSource> : IBehaviourNodeAsset<TSource>
    {
#if ODIN_INSPECTOR
        [GUIColor(1f, 0.92156863f, 0.015686275f)]
#endif
        [SerializeField]
        private string name;

        public string Name => this.name;
        
        public BehaviourNode<TSource> Create() => this.Create(this.name);

        protected abstract BehaviourNode<TSource> Create(string name);
    }
}