using System;
using UnityEngine.Playables;

namespace UnityEngine.Timeline
{
    public class TimelineEventReceiver : MonoBehaviour, INotificationReceiver
    {
        [SerializeField]
        protected Reaction[] reactions;

        protected virtual void Awake()
        {
            foreach (var reaction in this.reactions)
            {
                reaction.Initialize();
            }
        }

        public virtual void OnNotify(Playable origin, INotification notification, object context)
        {
            PropertyName notificationId = notification.id;
           
            foreach (Reaction reaction in this.reactions)
            {
                if (reaction.Equals(notificationId))
                {
                    reaction.Invoke();
                }
            }
        }
        
        [Serializable]
        protected class Reaction
        {
            [SerializeField]
            protected string name;
            
            [SerializeReference]
            protected ITimelineAction[] actions = default;

            private PropertyName id;

            internal void Initialize()
            {
                this.id = new PropertyName(this.name);
            }

            internal bool Equals(PropertyName otherId)
            {
                return this.id == otherId;
            }

            internal void Invoke()
            {
                for (int i = 0, count = this.actions.Length; i < count; i++)
                {
                    ITimelineAction action = this.actions[i];
                    if (action != null)
                    {
                        action.Invoke();
                    }
                }
            }
        }
    }
}