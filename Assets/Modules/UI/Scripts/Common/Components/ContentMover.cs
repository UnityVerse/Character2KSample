using Sirenix.OdinInspector;
using UnityEngine;

namespace Modules.UI
{
    public sealed class ContentMover : MonoBehaviour
    {
        [SerializeField]
        private RectTransform content;
        
        [Button]
        public void Move(Vector2 offset)
        {
            this.content.anchoredPosition += offset;
        }
    }
}