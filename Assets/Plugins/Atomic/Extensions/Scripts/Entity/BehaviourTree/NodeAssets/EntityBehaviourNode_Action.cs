using System;
using Atomic.AI;
using Atomic.Entities;
using UnityEngine;

namespace Atomic.Extensions
{
    [Serializable]
    public sealed class EntityBehaviourNodeAsset_Action : 
        BehaviourNodeAsset_Action<IEntity>,
        IEntityBehaviourNodeAsset
    {
        [SerializeReference]
        private IEntityAction[] actions = default;
        
        public override Action<IEntity> Action
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