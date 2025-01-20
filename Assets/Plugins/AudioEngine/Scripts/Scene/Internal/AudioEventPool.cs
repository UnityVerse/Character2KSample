using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AudioEngine
{
    internal sealed class AudioEventPool : IDisposable
    {
        private readonly AudioSystem audioSystem;

        [ShowInInspector, ReadOnly]
        private readonly Dictionary<string, (AudioEventBase, Queue<AudioEventBase>)> eventMap = new();

        internal AudioEventPool(AudioSystem audioSystem)
        {
            this.audioSystem = audioSystem;
        }

        internal void RegisterPrefab(string identifier, AudioEventBase prefab)
        {
            this.eventMap[identifier] = (prefab, new Queue<AudioEventBase>());
        }

        internal void UnregisterPrefab(string identifier)
        {
            if (!this.eventMap.Remove(identifier, out (AudioEventBase prefab, Queue<AudioEventBase> queue) tuple))
            {
                return;
            }

            foreach (var audioEvent in tuple.queue)
            {
                ScriptableObject.Destroy(audioEvent);
            }

            tuple.queue.Clear();
        }

        internal AudioEventBase Get(string identifier)
        {
            if (!this.eventMap.TryGetValue(identifier, out (AudioEventBase prefab, Queue<AudioEventBase> queue) tuple))
            {
                throw new Exception($"Event identifier {identifier} is not found!");
            }

            if (!tuple.queue.TryDequeue(out AudioEventBase @event))
            {
                @event = ScriptableObject.Instantiate(tuple.prefab);
                @event.Initialize(identifier, this.audioSystem);
            }

            @event.spawned = true;
            return @event;
        }

        internal bool TryGet(string identifier, out AudioEventBase @event)
        {
            if (!this.eventMap.TryGetValue(identifier, out (AudioEventBase prefab, Queue<AudioEventBase> queue) tuple))
            {
                @event = default;
                return false;
            }

            if (!tuple.queue.TryDequeue(out @event))
            {
                @event = ScriptableObject.Instantiate(tuple.prefab);
                @event.Initialize(identifier, this.audioSystem);
            }

            @event.spawned = true;
            return true;
        }

        internal void Release(AudioEventBase audioEvent)
        {
            string identifier = audioEvent.identifier;
            audioEvent.spawned = false;
            
            if (this.eventMap.TryGetValue(identifier, out (AudioEventBase prefab, Queue<AudioEventBase> queue) tuple))
            {
                tuple.queue.Enqueue(audioEvent);
            }
        }

        public void Dispose()
        {
            foreach ((AudioEventBase, Queue<AudioEventBase>) kv in this.eventMap.Values)
            {
                Queue<AudioEventBase> queue = kv.Item2;
                foreach (AudioEventBase audioEvent in queue)
                {
                    ScriptableObject.Destroy(audioEvent);
                }
                
                queue.Clear();
            }
            
            this.eventMap.Clear();
        }
    }
}