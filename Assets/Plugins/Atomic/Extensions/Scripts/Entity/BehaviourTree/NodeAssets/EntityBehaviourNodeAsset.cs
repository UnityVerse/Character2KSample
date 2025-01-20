using System;
using Atomic.AI;
using Atomic.Entities;

namespace Atomic.Extensions
{
    [Serializable]
    public abstract class EntityBehaviourNodeAsset : BehaviourNodeAsset<IEntity>, IEntityBehaviourNodeAsset
    {
    }
}