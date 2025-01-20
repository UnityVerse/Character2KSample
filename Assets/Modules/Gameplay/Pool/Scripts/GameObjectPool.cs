using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Modules.Gameplay
{
    public sealed class GameObjectPool
    {
        [ShowInInspector, ReadOnly]
        private readonly Dictionary<string, Queue<GameObject>> queue = new();

        private readonly Transform poolContainer;
        private readonly IGameObjectFactory objectFactory;

        public GameObjectPool(Transform poolContainer, IGameObjectFactory factory)
        {
            this.poolContainer = poolContainer;
            this.objectFactory = factory;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Get<T>(T prefab, Transform parent = null) where T : Component
        {
            return this.Get(prefab.gameObject, parent).GetComponent<T>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Get<T>(T prefab, Vector3 position, Quaternion rotation, Transform parent) where T : Component
        {
            return this.Get(prefab.gameObject, position, rotation, parent).GetComponent<T>();
        }
        
        public GameObject Get(GameObject prefab, Transform parent)
        {
            string objectName = prefab.name;

            if (!this.queue.TryGetValue(objectName, out Queue<GameObject> queue))
            {
                queue = new Queue<GameObject>();
                this.queue.Add(objectName, queue);
            }

            if (queue.TryDequeue(out GameObject obj))
            {
                Transform transform = obj.transform;
                transform.parent = parent;
            }
            else
            {
                obj = this.objectFactory.Instantiate(objectName, prefab, Vector3.zero, Quaternion.identity, parent);
            }

            return obj;
        }

        public GameObject Get(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent)
        {
            string objectName = prefab.name;

            if (!this.queue.TryGetValue(objectName, out Queue<GameObject> queue))
            {
                queue = new Queue<GameObject>();
                this.queue.Add(objectName, queue);
            }

            if (queue.TryDequeue(out GameObject obj))
            {
                Transform transform = obj.transform;
                transform.parent = parent;
                transform.position = position;
                transform.rotation = rotation;
            }
            else
            {
                obj = objectFactory.Instantiate(objectName, prefab, position, rotation, parent);
            }

            return obj;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Release(Component obj, bool inContainer = false)
        {
            this.Release(obj.gameObject, inContainer);
        }

        public void Release(GameObject obj, bool inContainer = false)
        {
            Queue<GameObject> queue = this.queue[obj.name];
            queue.Enqueue(obj);

            if (inContainer)
            {
                obj.transform.SetParent(this.poolContainer);
            }
        }
    }
}