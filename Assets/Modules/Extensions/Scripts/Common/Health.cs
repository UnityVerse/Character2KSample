using System;
using Atomic.Elements;
using Atomic.Entities;
using Sirenix.OdinInspector;
using UnityEngine;
using Action = System.Action;

namespace Modules.Extensions
{
    [Serializable, InlineProperty]
    public sealed class Health
    {
        public event Action OnStateChanged;
        public event System.Action<int> OnHealthChanged;
        public event System.Action<int> OnMaxHealthChanged;
        public event Action OnHealthEmpty;

        [SerializeField, Min(0), MaxValue(nameof(max))]
        private int current;

        [SerializeField, Min(0)]
        private int max;

        public Health()
        {
        }

        public Health(int health)
        {
            this.max = health;
            this.current = health;
        }

        public Health(int health, int max)
        {
            this.max = max;
            this.current = Math.Clamp(health, 0, this.max);
        }

        public int GetMissing()
        {
            return this.max - this.current;
        }

        public int GetCurrent()
        {
            return this.current;
        }

        public int GetMax()
        {
            return this.max;
        }

        public bool IsEmpty()
        {
            return this.current == 0;
        }

        public bool IsFull()
        {
            return this.current == this.max;
        }

        public bool IsExists()
        {
            return this.current > 0;
        }

        public float GetPercent()
        {
            return (float) this.current / this.max;
        }

        [Button, HideInEditorMode]
        public bool Add(int range)
        {
            if (range <= 0)
            {
                return false;
            }
            
            if (this.current == this.max)
            {
                return false;
            }
            
            this.current = Math.Min(this.current + range, this.max);
            this.OnStateChanged?.Invoke();
            this.OnHealthChanged?.Invoke(this.current);

            return true;
        }

        [Button, HideInEditorMode]
        public bool Reduce(int range)
        {
            if (range < 0)
            {
                throw new Exception($"Range can't be less than zero! Actual range {range}");
            }

            if (this.current == 0)
            {
                return false;
            }

            if (range == 0)
            {
                return true;
            }

            this.current = Math.Max(0, this.current - range);
            this.OnStateChanged?.Invoke();
            this.OnHealthChanged?.Invoke(this.current);

            if (this.current == 0)
            {
                this.OnHealthEmpty?.Invoke();
            }
            
            return true;
        }

        [Button, HideInEditorMode]
        public void SetCurrent(int health)
        {
            if (this.current == health)
            {
                return;
            }

            this.current = Math.Clamp(health, 0, this.max);
            this.OnStateChanged?.Invoke();
            this.OnHealthChanged?.Invoke(this.current);
            
            if (this.current == 0)
            {
                this.OnHealthEmpty?.Invoke();
            }
        }

        [Button, HideInEditorMode]
        public void SetMax(int maxHealth)
        {
            if (this.max == maxHealth)
            {
                return;
            }

            this.max = Math.Max(1, maxHealth);
            this.OnMaxHealthChanged?.Invoke(this.max);

            int newHealth = Math.Min(this.current, this.max);
            if (newHealth != this.current)
            {
                this.current = newHealth;
                this.OnHealthChanged?.Invoke(newHealth);
            }
            
            this.OnStateChanged?.Invoke();
        }
    }
}