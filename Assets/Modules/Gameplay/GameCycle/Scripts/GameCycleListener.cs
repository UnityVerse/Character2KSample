using System;
using Atomic.Elements;
using Sirenix.OdinInspector;
using UnityEngine;
// ReSharper disable UnassignedField.Local

namespace Modules.Gameplay
{
    [DisallowMultipleComponent]
    [AddComponentMenu("Modules/Gameplay/Game Cycle Listener")]
    public sealed class GameCycleListener : MonoBehaviour,
        IGameStartListener,
        IGamePauseListener,
        IGameResumeListener,
        IGameFinishListener
    {
        [SerializeField]
        private bool off;

        [Header("Actions")]
        [DisableIf(nameof(off))]
        [SerializeField]
        private Action[] startActions;

        [DisableIf(nameof(off))]
        [SerializeField]
        private Action[] pauseActions;

        [DisableIf(nameof(off))]
        [SerializeField]
        private Action[] resumeActions;

        [DisableIf(nameof(off))]
        [SerializeField]
        private Action[] finishActions;

        [Header("Listeners")]
        [SerializeReference]
        private IGameListener[] listeners;

        public void OnStartGame()
        {
            if (this.startActions != null)
            {
                for (int i = 0, count = this.startActions.Length; i < count; i++)
                {
                    this.startActions[i].Invoke();
                }
            }

            if (this.listeners != null)
            {
                for (int i = 0, count = this.listeners.Length; i < count; i++)
                {
                    if (this.listeners[i] is IGameStartListener startListener)
                    {
                        startListener.OnStartGame();
                    }
                }
            }
        }

        public void OnPauseGame()
        {
            if (this.pauseActions != null)
            {
                for (int i = 0, count = this.pauseActions.Length; i < count; i++)
                {
                    this.pauseActions[i].Invoke();
                }
            }
            
            if (this.listeners != null)
            {
                for (int i = 0, count = this.listeners.Length; i < count; i++)
                {
                    if (this.listeners[i] is IGamePauseListener pauseListener)
                    {
                        pauseListener.OnPauseGame();
                    }
                }
            }
        }

        public void OnResumeGame()
        {
            if (this.resumeActions != null)
            {
                for (int i = 0, count = this.resumeActions.Length; i < count; i++)
                {
                    this.resumeActions[i].Invoke();
                }
            }
            
            if (this.listeners != null)
            {
                for (int i = 0, count = this.listeners.Length; i < count; i++)
                {
                    if (this.listeners[i] is IGameResumeListener resumeListener)
                    {
                        resumeListener.OnResumeGame();
                    }
                }
            }
        }

        public void OnFinishGame()
        {
            if (this.finishActions != null)
            {
                for (int i = 0, count = this.finishActions.Length; i < count; i++)
                {
                    this.finishActions[i].Invoke();
                }
            }
            
            if (this.listeners != null)
            {
                for (int i = 0, count = this.listeners.Length; i < count; i++)
                {
                    if (this.listeners[i] is IGameFinishListener finishListener)
                    {
                        finishListener.OnFinishGame();
                    }
                }
            }
        }

        [Serializable]
        private struct Action
        {
            [SerializeField]
            private bool off;

            [DisableIf(nameof(off))]
            [SerializeReference]
            private IAction value;

            public void Invoke()
            {
                if (!this.off)
                {
                    this.value?.Invoke();
                }
            }
        }
    }
}