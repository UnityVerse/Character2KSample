using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Modules.UI
{
    public class ViewList<T> : MonoBehaviour, IEnumerable<T> where T : Component
    {
        public event Action<T, int> OnItemSpawned;
        public event Action<T, int> OnItemUnspawned;
        public event Action OnStateChanged;

        [SerializeField]
        private Transform viewport;
        
        [SerializeField]
        private T itemPrefab;

        [Space]
        [ShowInInspector, ReadOnly]
        private List<T> items = new();
        
        private readonly Queue<T> recycledItems = new();

        public T SpawnItem()
        {
            int index = this.items.Count;
            return this.SpawnItemAt(index);
        }

        public T SpawnItemAt(int index)
        {
            if (index < 0 || index > this.items.Count)
            {
                throw new Exception($"Invalid index {index}!");
            }

            if (!this.recycledItems.TryDequeue(out T item))
            {
                item = Instantiate(this.itemPrefab, this.viewport);
            }

            item.transform.SetSiblingIndex(index);
            this.OnSpawnItem(item);

            this.items.Insert(index, item);
            this.OnItemSpawned?.Invoke(item, index);
            this.OnStateChanged?.Invoke();
            return item;
        }

        public bool UnspawnItem(T item)
        {
            int index = this.items.IndexOf(item);
            if (index == -1)
            {
                return false;
            }

            this.items.RemoveAt(index);
            this.OnUnspawnItem(item);
            
            this.recycledItems.Enqueue(item);
            this.OnItemUnspawned?.Invoke(item, index);
            this.OnStateChanged?.Invoke();
            return true;
        }
        
        public IReadOnlyList<T> GetItems()
        {
            return this.items;
        }

        public void ClearItems()
        {
            for (int i = 0, count = this.items.Count; i < count; i++)
            {
                T item = this.items[i];
                this.OnUnspawnItem(item);
                this.recycledItems.Enqueue(item);
            }

            this.items.Clear();
            this.OnStateChanged?.Invoke();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return this.items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        
        protected virtual void OnSpawnItem(T item)
        {
        }

        protected virtual void OnUnspawnItem(T item)
        {
        }
    }
}