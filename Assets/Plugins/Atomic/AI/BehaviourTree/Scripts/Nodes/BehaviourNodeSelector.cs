using System.Collections.Generic;

namespace Atomic.AI
{
    public class BehaviourNodeSelector<TSource> : BehaviourNodeComposite<TSource>
    {
        private int nodeIndex;

        public BehaviourNodeSelector(string name, IEnumerable<BehaviourNode<TSource>> nodes) : base(name, nodes)
        {
        }

        protected override void OnEnable(TSource source)
        {
            this.nodeIndex = 0;
        }

        protected override void OnAbort(TSource source)
        {
            BehaviourNode<TSource> currentNode = this.nodes[this.nodeIndex];
            currentNode.Abort(source);
        }

        protected override State OnUpdate(TSource source, float deltaTime)
        {
            BehaviourNode<TSource> currentNode = this.nodes[this.nodeIndex];
            State result = currentNode.Run(source, deltaTime);

            if (result != State.FAILURE)
            {
                return result;
            }

            //Failure:
            if (this.nodeIndex == this.nodes.Count - 1)
            {
                return State.FAILURE;
            }

            this.nodeIndex++;
            return State.RUNNING;
        }
    }
}