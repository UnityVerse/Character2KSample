using Atomic.AI;
using Atomic.Entities;

namespace Atomic.Extensions
{
    public abstract class EntityBehaviourNode : BehaviourNode<IEntity>
    {
        protected EntityBehaviourNode(string name) : base(name)
        {
        }
    }
}