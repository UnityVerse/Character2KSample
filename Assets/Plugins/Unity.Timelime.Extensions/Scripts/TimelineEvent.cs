using UnityEngine.Playables;

namespace UnityEngine.Timeline
{
    public sealed class TimelineEvent : Marker, INotification
    {
        [SerializeField]
        public string eventName;
        
        public PropertyName id => this.eventName;
    }
}