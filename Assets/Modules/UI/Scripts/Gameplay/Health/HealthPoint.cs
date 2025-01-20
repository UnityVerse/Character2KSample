using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Modules.UI
{
    public sealed class HealthPoint : MonoBehaviour
    {
        [SerializeField]
        private Image image;
        
        [SerializeField]
        private Color activeColor = new(0.85f, 0.85f, 0.85f, 0.5f);

        [SerializeField]
        private Color inactiveColor = new(0.85f, 0.85f, 0.85f, 0.25f);

        [SerializeField]
        private float activationDuration = 0.15f;

        public void SetVisible(bool visible)
        {
            this.gameObject.SetActive(visible);
        }
        
        public void SetActive(bool active)
        {
            this.image.color = active ? this.activeColor : this.inactiveColor;
        }

        public void Activate()
        {
            this.image.DOColor(this.activeColor, this.activationDuration);
        }
    }
}