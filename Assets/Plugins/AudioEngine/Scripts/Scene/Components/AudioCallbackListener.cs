using UnityEngine;
using UnityEngine.Events;

namespace AudioEngine
{
    [AddComponentMenu("Audio/Audio Callback Listener")]
    public sealed class AudioCallbackListener : MonoBehaviour
    {
        [SerializeField]
        private string callback;

        [SerializeField]
        private UnityEvent action;
        
        private void OnEnable()
        {
            AudioSystem.Instance.ListenCallback(this.callback, this.action.Invoke);
        }

        private void OnDisable()
        {
            AudioSystem.Instance.UnlistenCallback(this.callback,this.action.Invoke);
        }
    }
}