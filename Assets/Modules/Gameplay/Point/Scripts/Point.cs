using System;
using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace Modules.Gameplay
{
    [Serializable]
    public abstract class Point<T> where T : class
    {
        public event Action OnStateChanged;

#if ODIN_INSPECTOR
        [ShowInInspector]
#endif
        private readonly string name;

#if ODIN_INSPECTOR
        [ShowInInspector]
#endif
        private T value;

        protected Point(string name, T value = null)
        {
            this.name = name;
            this.value = value;
        }
        
        public abstract Vector3 GetPosition();
        public abstract Quaternion GetRotation();
        
        public bool IsValue(T target)
        {
            return this.value == target;
        }
        
        public T GetValue()
        {
            return this.value;
        }

        public bool IsOccuped()
        {
            return this.value != null;
        }

        public bool IsFree()
        {
            return this.value == null;
        }
        
        public void SetValue(T target)
        {
            if (target != this.value)
            {
                this.value = target;
                this.OnStateChanged?.Invoke();
            }
        }

        public void ResetValue()
        {
            if (this.value != null)
            {
                this.value = null;
                this.OnStateChanged?.Invoke();
            }
        }

        public string GetName()
        {
            return this.name;
        }
    }
}