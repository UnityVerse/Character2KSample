using System;
using Atomic.Elements;
using DG.Tweening;
using UnityEngine;

namespace Modules.Extensions
{
    [Serializable]
    public sealed class CanvasAlphaAnimationAction : IAction
    {
        [SerializeField]
        private float initialAlpha = 0.5f;

        [SerializeField]
        private float targetAlpha = 1.0f;

        [SerializeField]
        private float duration = 0.5f;

        [Space]
        [SerializeField]
        private CanvasGroup canvasGroup;

        public void Invoke()
        {
            this.canvasGroup.alpha = this.initialAlpha;
            this.canvasGroup.DOFade(this.targetAlpha, this.duration);
        }
    }
}