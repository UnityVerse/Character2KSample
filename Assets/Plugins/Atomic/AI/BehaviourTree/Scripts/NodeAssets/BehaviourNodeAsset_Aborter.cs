using System;

namespace Atomic.AI
{
    [Serializable]
    public abstract class BehaviourNodeAsset_Aborter<TSource> : BehaviourNodeAsset<TSource>
    {
        protected abstract Predicate<TSource>[] Conditions { get; }
        protected abstract Action<TSource> Action { get; }
        protected abstract IBehaviourNodeAsset<TSource> Node { get; }

        protected sealed override BehaviourNode<TSource> Create(string name)
        {
            return new BehaviourNodeAborter<TSource>(name, this.Node.Create(), this.Conditions, this.Action);
        }
    }
}