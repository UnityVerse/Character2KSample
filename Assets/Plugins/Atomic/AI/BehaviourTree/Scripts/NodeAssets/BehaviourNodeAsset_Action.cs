using System;

namespace Atomic.AI
{
    [Serializable]
    public abstract class BehaviourNodeAsset_Action<TSource> : BehaviourNodeAsset<TSource>
    {
        public abstract Action<TSource> Action { get; }

        protected sealed override BehaviourNode<TSource> Create(string name)
        {
            return new BehaviourNodeAction<TSource>(name, this.Action);
        }
    }
}