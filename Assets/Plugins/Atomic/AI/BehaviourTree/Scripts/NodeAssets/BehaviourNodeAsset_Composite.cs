using System;
using System.Collections.Generic;

namespace Atomic.AI
{
    [Serializable]
    public abstract class BehaviourNodeAsset_Composite<TSource> : BehaviourNodeAsset<TSource>
    {
        public abstract IEnumerable<IBehaviourNodeAsset<TSource>> Children { get; } 

        protected sealed override BehaviourNode<TSource> Create(string name)
        {
            return this.Create(name, this.GetChildren());
        }

        protected abstract BehaviourNode<TSource> Create(string name, IEnumerable<BehaviourNode<TSource>> children);

        private IEnumerable<BehaviourNode<TSource>> GetChildren()
        {
            IEnumerable<IBehaviourNodeAsset<TSource>> children = this.Children;
            if (children == null)
            {
                yield break;
            }
            
            foreach (IBehaviourNodeAsset<TSource> child in children)
            {
                yield return child.Create();
            }
        }
    }
}