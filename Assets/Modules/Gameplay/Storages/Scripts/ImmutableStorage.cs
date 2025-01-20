using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Modules.Gameplay
{
    public sealed class ImmutableStorage<TKey> : IEnumerable<Cell<TKey>>
    {
        public event Action OnStateChanged;

        [ShowInInspector]
        private readonly Dictionary<TKey, Cell<TKey>> cells;

        public int CellCount => this.cells.Count;

        public ImmutableStorage(params TKey[] keys)
        {
            this.cells = keys.ToDictionary(it => it, key => new Cell<TKey>(key, 0));
        }

        public ImmutableStorage(Dictionary<TKey, int> cells)
        {
            this.cells = cells.ToDictionary(it => it.Key, it => new Cell<TKey>(it.Key, it.Value));
        }

        public ImmutableStorage(params KeyValuePair<TKey, int>[] items)
        {
            this.cells = items.ToDictionary(it => it.Key, it => new Cell<TKey>(it.Key, it.Value));
        }

        public void AddAmount(TKey key, int range)
        {
            if (range <= 0)
            {
                return;
            }

            if (this.cells.TryGetValue(key, out Cell<TKey> cell))
            {
                cell.Count += range;
            }
            else
            {
                throw new Exception($"Cell with key {key} is not found!");
            }

            this.OnStateChanged?.Invoke();
        }

        public void RemoveOne(TKey key)
        {
            this.RemoveAmount(key, 1);
        }

        public bool RemoveAmount(TKey key, int range)
        {
            if (range <= 0)
            {
                return false;
            }

            if (!this.cells.TryGetValue(key, out Cell<TKey> cell))
            {
                Debug.LogWarning($"Can't remove items with name: {key}. Not exists!");
                return false;
            }

            if (cell.Count < range)
            {
                Debug.LogWarning($"Can't remove items with name: {key}. Range {range} more then count {cell.Count}!");
                return false;
            }

            cell.Count -= range;
            this.OnStateChanged?.Invoke();
            return true;
        }

        public bool ExistsAny(TKey key)
        {
            return this.ExistsAmount(key, 1);
        }

        public bool ExistsAmount(TKey key, int minCount)
        {
            return this.cells.TryGetValue(key, out Cell<TKey> cell) && cell.Count >= minCount;
        }

        public int GetAmount(TKey key)
        {
            if (this.cells.TryGetValue(key, out Cell<TKey> cell))
            {
                return cell.Count;
            }

            return 0;
        }

        public void Clear()
        {
            if (this.cells.Count > 0)
            {
                this.cells.Clear();
                this.OnStateChanged?.Invoke();
            }
        }

        public IEnumerator<Cell<TKey>> GetEnumerator()
        {
            return this.cells.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}