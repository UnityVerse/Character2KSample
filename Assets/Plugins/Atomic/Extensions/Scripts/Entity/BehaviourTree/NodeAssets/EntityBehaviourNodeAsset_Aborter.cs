using System;
using Atomic.AI;
using Atomic.Entities;
using UnityEngine;

namespace Atomic.Extensions
{
    [Serializable]
    public sealed class EntityBehaviourNodeAsset_Aborter :
        BehaviourNodeAsset_Aborter<IEntity>,
        IEntityBehaviourNodeAsset
    {
        [Header("Conditions")]
        [SerializeReference]
        private IEntityPredicate[] conditions = default;

        [Header("Actions")]
        [SerializeReference]
        private IEntityAction[] actions = default;

        [Header("Node")]
        [SerializeReference]
        private IEntityBehaviourNodeAsset node = default;
        
        protected override IBehaviourNodeAsset<IEntity> Node
        {
            get { return this.node; }
        }

        protected override Predicate<IEntity>[] Conditions
        {
            get
            {
                if (this.conditions == null)
                {
                    return Array.Empty<Predicate<IEntity>>();
                }

                int count = this.conditions.Length;
                var conditions = new Predicate<IEntity>[count];
                for (int i = 0; i < count; i++)
                {
                    conditions[i] = this.conditions[i].Invoke;
                }

                return conditions;
            }
        }

        protected override Action<IEntity> Action
        {
            get
            {
                return e =>
                {
                    if (this.actions == null)
                    {
                        return;
                    }

                    for (int i = 0, count = this.actions.Length; i < count; i++)
                    {
                        this.actions[i]?.Invoke(e);
                    }
                };
            }
        }
    }
}