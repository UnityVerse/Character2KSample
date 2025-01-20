using System;
using Atomic.AI;
using Atomic.Entities;
using UnityEngine;

namespace Atomic.Extensions
{
    [Serializable]
    public sealed class EntityBehaviourNodeAsset_Condition : 
        BehaviourNodeAsset_Condition<IEntity>,
        IEntityBehaviourNodeAsset
    {
        public override Predicate<IEntity> Condition => e => this.conditions.All(e);

        [SerializeReference]
        private IEntityPredicate[] conditions = default;
    }
}