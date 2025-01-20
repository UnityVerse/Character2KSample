using DG.Tweening;
using UnityEngine;

namespace Modules.Extensions
{
    public sealed class LightAnimation : MonoBehaviour
    {
        [SerializeField]
        private new Light light;

        [SerializeField]
        private int startIntensity;

        [SerializeField]
        private float endIntensity = 1;

        [SerializeField]
        private float duration = 0.5f;

        private void Start()
        {
            this.light.DOIntensity(this.endIntensity, duration).SetLoops(-1, LoopType.Yoyo);
        }
    }
}