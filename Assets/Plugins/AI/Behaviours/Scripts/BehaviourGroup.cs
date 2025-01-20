using System.Collections.Generic;
using UnityEngine;

namespace Atomic.AI
{
    public sealed class BehaviourGroup
    {
        private readonly IBlackboard blackboard;
        private bool started;
        
        private readonly List<IAIBehaviour> behaviours = new();
        private readonly List<IUpdateAIBehaviour> updateBehaviours = new();
        private readonly List<IUpdateAIBehaviour> _updateCache = new();

        public bool IsStarted => this.started;
        public IReadOnlyList<IAIBehaviour> AllBehaviours => this.behaviours;
        
        public BehaviourGroup(IBlackboard blackboard)
        {
            this.blackboard = blackboard;
        }

        public void OnStart()
        {
            if (this.started)
            {
                return;
            }

            this.started = true;
            
            for (int i = 0, count = this.behaviours.Count; i < count; i++)
            {
                if (this.behaviours[i] is IStartAIBehaviour behaviour)
                {
                    behaviour.OnStart(this.blackboard);
                }
            }
        }

        public void OnStop()
        {
            if (!this.started)
            {
                return;
            }
            
            for (int i = 0, count = this.behaviours.Count; i < count; i++)
            {
                if (this.behaviours[i] is IStopAIBehaviour behaviour)
                {
                    behaviour.OnStop(this.blackboard);
                }
            }
            
            this.started = false;
        }

        public void OnUpdate(float deltaTime)
        {
            if (!this.started)
            {
                Debug.LogWarning("Behaviour System is not started!");
                return;
            }
            
            if (this.updateBehaviours.Count == 0)
            {
                return;
            }

            _updateCache.Clear();
            _updateCache.AddRange(this.updateBehaviours);

            for (int i = 0, count = _updateCache.Count; i < count; i++)
            {
                IUpdateAIBehaviour logic = _updateCache[i];
                logic.OnUpdate(this.blackboard, deltaTime);
            }
        }
        
        public bool AddBehaviour(IAIBehaviour target)
        {
            if (target == null)
            {
                return false;
            }

            if (this.behaviours.Contains(target))
            {
                return false;
            }

            this.behaviours.Add(target);

            if (this.started && target is IStartAIBehaviour behaviour)
            {
                behaviour.OnStart(this.blackboard);
            }
            
            if (target is IUpdateAIBehaviour updateBehaviour)
            {
                this.updateBehaviours.Add(updateBehaviour);
            }
            
            return true;
        }

        public bool DelBehaviour(IAIBehaviour target)
        {
            if (target == null)
            {
                return false;
            }

            if (!this.behaviours.Remove(target))
            {
                return false;
            }
            
            if (target is IUpdateAIBehaviour updateBehaviour)
            {
                this.updateBehaviours.Remove(updateBehaviour);
            }
            
            if (this.started && target is IStopAIBehaviour behaviour)
            {
                behaviour.OnStop(this.blackboard);
            }
            
            return true;
        }
    }
}