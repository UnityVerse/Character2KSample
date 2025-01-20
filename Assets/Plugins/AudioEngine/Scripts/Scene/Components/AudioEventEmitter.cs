using Sirenix.OdinInspector;
using UnityEngine;

namespace AudioEngine
{
    [AddComponentMenu("Audio/Audio Event Emitter")]
    public sealed class AudioEventEmitter : MonoBehaviour
    {
        [SerializeField]
        private AudioEventKey eventKey;

        [SerializeField]
        private float threshold;

        [SerializeField]
        private Transform soundPoint;

        [Button, GUIColor(1f, 0.83f, 0f), HideInEditorMode]
        public void PlayEvent()
        {
            AudioSystem.Instance.PlayEvent(this.eventKey, this.soundPoint.position, this.soundPoint.rotation, this.threshold);
        }

        private void Reset()
        {
            this.soundPoint = this.transform;
        }
    }
}