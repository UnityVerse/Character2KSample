using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Modules.Gameplay
{
    public sealed class PolygonInput : MonoBehaviour
    {
        public event Action OnStarted;
        public event Action OnFinished;
        
        [SerializeField]
        private float minPixels = 1;
        
        [ShowInInspector, ReadOnly]
        private readonly List<Vector2> points = new();

        private bool started;

        private readonly List<Func<bool>> conditions = new();

        public void AddCondition(Func<bool> condition)
        {
            this.conditions.Add(condition);
        }

        public void DelCondition(Func<bool> condition)
        {
            this.conditions.Remove(condition);
        }

        public IReadOnlyList<Vector2> GetPoints()
        {
            return this.points;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0) && 
                this.conditions.All(it => it.Invoke()))
            {
                this.points.Clear();
                this.OnStarted?.Invoke();
                this.started = true;
            }

            if (!this.started)
            {
                return;
            }

            if (Input.GetMouseButton(0))
            {
                Vector2 pixelPosition = Input.mousePosition;
                if (this.points.Count == 0 ||
                    Vector2.Distance(pixelPosition, this.points[^1]) > this.minPixels)
                {
                    this.points.Add(pixelPosition);
                }
                
            } else if (Input.GetMouseButtonUp(0))
            {
                this.started = false;
                this.OnFinished?.Invoke();
            }
        }
    }
}