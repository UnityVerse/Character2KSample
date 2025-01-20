using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Modules.UI
{
    public sealed class InventoryItem : MonoBehaviour
    {
        public event UnityAction<InventoryItem> OnClicked;

        [SerializeField]
        private Image icon;

        [SerializeField]
        private TMP_Text amount;

        [SerializeField]
        private Button button;

        [SerializeField]
        private GameObject endless;

        [SerializeField]
        private Color selectedColor = new(1f, 0.93f, 0.28f);

        [SerializeField]
        private Color deselectedColor = Color.white;

        private void OnEnable()
        {
            this.button.onClick.AddListener(this.OnClick);
        }

        private void OnDisable()
        {
            this.button.onClick.RemoveListener(this.OnClick);
        }

        public void SetIcon(Sprite icon)
        {
            this.icon.sprite = icon;
        }

        public void SetSelected(bool selected)
        {
            this.icon.color = selected ? this.selectedColor : this.deselectedColor;
        }

        public void SetAmount(string count)
        {
            this.amount.text = count;
        }

        public void SetEndless(bool active)
        {
            this.endless.SetActive(active);
            this.amount.enabled = !active;
        }

        private void OnClick()
        {
            this.OnClicked?.Invoke(this);
        }
    }
}