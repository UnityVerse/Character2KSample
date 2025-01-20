using System;
using System.Collections.Generic;

namespace Atomic.AI
{
    [Serializable]
    public abstract class BehaviourNodeAsset_Sequence<TSource> : BehaviourNodeAsset_Composite<TSource>
    {
        protected sealed override BehaviourNode<TSource> Create(string name, IEnumerable<BehaviourNode<TSource>> children)
        {
            return new BehaviourNodeSequence<TSource>(name, children);
        }
    }
}