using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace Atomic.AI
{
    public abstract class BehaviourNodeComposite<TSource> : BehaviourNode<TSource>
    {
        [ShowInInspector, ReadOnly, HideInEditorMode]
        protected readonly List<BehaviourNode<TSource>> nodes;

        protected BehaviourNodeComposite(string name, IEnumerable<BehaviourNode<TSource>> nodes) : base(name)
        {
            this.nodes = new List<BehaviourNode<TSource>>(nodes);
        }

        public override BehaviourNode<TSource> FindChild(string name)
        {
            for (int i = 0, count = this.nodes.Count; i < count; i++)
            {
                BehaviourNode<TSource> node = this.nodes[i];
                if (name.Equals(node.Name))
                {
                    return node;
                }
            }
            
            for (int i = 0, count = this.nodes.Count; i < count; i++)
            {
                BehaviourNode<TSource> child = this.nodes[i].FindChild(name);
                if (child != null)
                {
                    return child;
                }
            }
            
            return null;
        }
    }
}