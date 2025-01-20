using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Modules.UI
{
    public sealed class HealthBar : MonoBehaviour
    {
        [SerializeField]
        private HealthPoint[] healthPoints;
        
        [FoldoutGroup("Debug")]
        [ShowInInspector, ReadOnly]
        private int health;

        [FoldoutGroup("Debug")]
        [ShowInInspector, ReadOnly]
        private int maxHealth;

        public void Show()
        {
            this.gameObject.SetActive(true);
        }

        public void Hide()
        {
            this.gameObject.SetActive(false);
        }

        [FoldoutGroup("Debug")]
        [Button]
        public void SetHealth(int health)
        {
            int pointCount = this.healthPoints.Length;
            health = Math.Min(pointCount, health);

            for (int i = 0; i < health; i++)
            {
                HealthPoint healthPoint = this.healthPoints[i];
                healthPoint.SetActive(true);
            }

            for (int i = health; i < pointCount; i++)
            {
                HealthPoint healthPoint = this.healthPoints[i];
                healthPoint.SetActive(false);
            }

            this.health = health;
        }

        [FoldoutGroup("Debug")]
        [Button]
        public void SetMaxHealth(int maxHealth)
        {
            int pointCount = this.healthPoints.Length;
            maxHealth = Math.Min(pointCount, maxHealth);

            for (int i = 0; i < maxHealth; i++)
            {
                HealthPoint healthPoint = this.healthPoints[i];
                healthPoint.SetVisible(true);
            }

            for (int i = maxHealth; i < pointCount; i++)
            {
                HealthPoint healthPoint = this.healthPoints[i];
                healthPoint.SetVisible(false);
            }

            this.maxHealth = maxHealth;
        }
        
        public int GetHealth()
        {
            return this.health;
        }

        public int GetMaxHealth()
        {
            return this.maxHealth;
        }

    }
}