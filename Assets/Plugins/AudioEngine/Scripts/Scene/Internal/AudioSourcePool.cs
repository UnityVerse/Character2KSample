using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

// ReSharper disable UseObjectOrCollectionInitializer

namespace AudioEngine
{
    internal sealed class AudioSourcePool : IDisposable
    {
        private const string INACTIVE_NAME = "<audio_source_inactive>";
        private const string ACTIVE_NAME = "<audio_source_active>";

        private const int INITIAL_SIZE = 4;

        private readonly Transform parent;

        [ShowInInspector, ReadOnly]
        private readonly Queue<AudioSource> availableSources = new();

        internal AudioSourcePool(Transform parent)
        {
            this.parent = parent;

            for (int i = 0; i < INITIAL_SIZE; i++)
            {
                GameObject sourceGO = new GameObject();
                sourceGO.name = INACTIVE_NAME;
                sourceGO.transform.parent = parent;

                AudioSource source = sourceGO.AddComponent<AudioSource>();
                source.playOnAwake = false;
                source.enabled = false;
                this.availableSources.Enqueue(source);
            }
        }

        public void Dispose()
        {
            foreach (var source in this.availableSources)
            {
                GameObject.Destroy(source.gameObject);
            }

            this.availableSources.Clear();
        }

        internal AudioSource Take()
        {
            AudioSource source;

            if (this.availableSources.Count > 0)
            {
                source = this.availableSources.Dequeue();
            }
            else
            {
                GameObject sourceGO = new GameObject();
                sourceGO.transform.parent = this.parent;

                source = sourceGO.AddComponent<AudioSource>();
                source.playOnAwake = false;
            }

            source.enabled = true;
            source.name = ACTIVE_NAME;
            return source;
        }

        internal void Release(AudioSource source)
        {
            source.enabled = false;
            source.name = INACTIVE_NAME;
            source.transform.parent = parent;
            this.availableSources.Enqueue(source);
        }
    }
}