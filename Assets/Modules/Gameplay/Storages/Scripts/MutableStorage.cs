using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Modules.Gameplay
{
    public class MutableStorage<TKey> : IEnumerable<Cell<TKey>>
    {
        public event Action<Cell<TKey>> OnCellAdded;
        public event Action<Cell<TKey>> OnCellRemoved;
        public event Action OnStateChanged;

        [ShowInInspector]
        private readonly Dictionary<TKey, Cell<TKey>> cells;

        public int CellCount => this.cells.Count;

        public MutableStorage()
        {
            this.cells = new Dictionary<TKey, Cell<TKey>>();
        }
        
        public MutableStorage(Dictionary<TKey, int> cells)
        {
            this.cells = cells.ToDictionary(it => it.Key, it => new Cell<TKey>(it.Key, it.Value));
        }

        public MutableStorage(params KeyValuePair<TKey, int>[] items)
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
                cell = new Cell<TKey>(key, range);
                this.cells.Add(key, cell);
                this.OnCellAdded?.Invoke(cell);
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

            if (cell.Count == 0)
            {
                this.cells.Remove(cell.Key);
                this.OnCellRemoved?.Invoke(cell);
            }

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

        public Cell<TKey> GetOrCreateCell(TKey key)
        {
            if (this.cells.TryGetValue(key, out Cell<TKey> cell))
            {
                return cell;
            }

            cell = new Cell<TKey>(key, 0);
            this.cells.Add(key, cell);
            this.OnCellAdded?.Invoke(cell);
            return cell;
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