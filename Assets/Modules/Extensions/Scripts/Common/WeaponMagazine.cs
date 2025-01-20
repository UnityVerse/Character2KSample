using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace Modules.Extensions
{
    [Serializable]
    public sealed class WeaponMagazine
    {
        public event Action OnStateChanged;
        public event Action<int> OnCountChanged;
        public event Action<int> OnCapacityChanged;

        [ShowInInspector, HideInEditorMode]
        public int Count => this.count;

        [ShowInInspector, HideInEditorMode]
        public int Capacity => this.capacity;

        [HideInPlayMode]
        [SerializeField, Min(0), MaxValue(nameof(capacity))]
        private int count;

        [HideInPlayMode]
        [SerializeField, Min(0)]
        private int capacity;

        public int GetCount()
        {
            return this.count;
        }

        public int GetCapacity()
        {
            return this.capacity;
        }

        public bool IsEmpty()
        {
            return this.count == 0;
        }

        public bool IsFull()
        {
            return this.count == this.capacity;
        }

        public bool BulletExists()
        {
            return this.count > 0;
        }

        public float GetPercent()
        {
            return (float) this.count / this.capacity;
        }

        [Button, HideInEditorMode]
        public bool AddBullets(int range)
        {
            if (range <= 0)
            {
                return false;
            }

            if (this.count == this.capacity)
            {
                return false;
            }

            this.count = Math.Min(this.count + range, this.capacity);
            this.OnStateChanged?.Invoke();
            this.OnCountChanged?.Invoke(this.count);
            return true;
        }

        [Button, HideInEditorMode]
        public void SpendBullet()
        {
            if (this.count == 0)
            {
                return;
            }

            this.count = Math.Max(0, this.count - 1);
            this.OnStateChanged?.Invoke();
            this.OnCountChanged?.Invoke(this.count);
        }

        [Button, HideInEditorMode]
        public void SetCount(int count)
        {
            if (this.count == count)
            {
                return;
            }

            this.count = Math.Clamp(count, 0, this.capacity);
            this.OnStateChanged?.Invoke();
            this.OnCountChanged?.Invoke(this.count);
        }

        [Button, HideInEditorMode]
        public void SetCapacity(int capacity)
        {
            if (this.capacity == capacity)
            {
                return;
            }

            this.capacity = Math.Max(1, capacity);
            this.OnCapacityChanged?.Invoke(this.capacity);

            int newCount = Math.Min(this.count, this.capacity);
            if (newCount != this.count)
            {
                this.count = newCount;
                this.OnCountChanged?.Invoke(newCount);
            }

            this.OnStateChanged?.Invoke();
        }
    }
}