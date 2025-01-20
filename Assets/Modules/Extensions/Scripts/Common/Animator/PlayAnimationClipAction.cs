using System;
using Animancer;
using Atomic.Elements;
using Atomic.Entities;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace Modules.Extensions
{
    [Serializable]
    public sealed class PlayAnimationClipAction : IAction
    {
        [SerializeField]
        private HybridAnimancerComponent animator;

        [SerializeField]
        private Mode mode = Mode.CLIP;

        [SerializeField, ShowIf(nameof(mode), Mode.CLIP)]
        private AnimationClip animation;

        [SerializeField, ShowIf(nameof(mode), Mode.CONTROLLER)]
        private RuntimeAnimatorController animatorController;

        [SerializeField, ShowIf(nameof(mode), Mode.LAYER)]
        private int stateIndex;

        [SerializeField]
        private float fadeDuration;

        [SerializeField]
        private float speed = 1;

        [SerializeField]
        private float delay;

        [SerializeField]
        private bool mute;

        private enum Mode
        {
            CLIP = 0,
            CONTROLLER = 1,
            LAYER = 2
        }

        public async void Invoke()
        {
            if (this.mute)
            {
                return;
            }

            if (animator == null)
            {
                return;
            }

            if (this.delay > 0)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(this.delay), DelayType.DeltaTime);
            }

            AnimancerState state = this.mode switch
            {
                Mode.CLIP => animator.States.GetOrCreate(this.animation),
                Mode.CONTROLLER => new ControllerState(this.animatorController),
                Mode.LAYER => animator.Layers[0].GetChild(this.stateIndex),
                _ => throw new ArgumentOutOfRangeException()
            };

            state.Speed = this.speed;


            animator.Play(state, this.fadeDuration);
        }
    }
}