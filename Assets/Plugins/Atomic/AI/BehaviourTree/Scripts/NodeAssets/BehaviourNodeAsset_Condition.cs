using System;

namespace Atomic.AI
{
    [Serializable]
    public abstract class BehaviourNodeAsset_Condition<TSource> : BehaviourNodeAsset<TSource>
    {
        public abstract Predicate<TSource> Condition { get; }

        protected sealed override BehaviourNode<TSource> Create(string name)
        {
            return new BehaviourNodeCondition<TSource>(name, this.Condition);
        }
    }
}