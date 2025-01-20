using System;
using JetBrains.Annotations;
using UnityEngine;

namespace Modules.Gameplay
{
    public sealed class InputDragger : MonoBehaviour
    {
        public event Action<Vector2> OnDrag;
        
        private bool isTouch;

        private Vector3 currentPosition;

        [SerializeField]
        private new Camera camera;
        
        [SerializeField]
        private float dragSpeed = 5.0f;

        [Range(0, 1)]
        [SerializeField]
        private float minDrag = 0.01f;

        public bool IsDragging => this.isDrag;
        
        private bool isDrag;

        #region Input

        private void Update()
        {
#if UNITY_EDITOR
            this.ProcessMouseEvents();
#elif UNITY_ANDROID
            this.ProcessTouchEvents();
#endif
        }

        private void ProcessMouseEvents()
        {
            if (Input.GetMouseButtonDown(0))
            {
                this.ProcessInputDown(Input.mousePosition);
            }
            else if (Input.GetMouseButton(0))
            {
                this.ProcessInputDrag(Input.mousePosition);
            }
            else if (Input.GetMouseButtonUp(0))
            {
                this.ProcessInputUp();
            }
        }

        [UsedImplicitly]
        private void ProcessTouchEvents()
        {
            var touchCount = Input.touchCount;
            if (touchCount != 1)
            {
                return;
            }

            var touch = Input.touches[0];
            var phase = touch.phase;
            if (phase == TouchPhase.Began)
            {
                this.ProcessInputDown(touch.position);
            }
            else if (phase == TouchPhase.Moved)
            {
                this.ProcessInputDrag(touch.position);
            }
            else if (phase == TouchPhase.Ended)
            {
                this.ProcessInputUp();
            }
        }

        private void ProcessInputDown(Vector2 inputPosition)
        {
            this.isTouch = true;
            this.currentPosition = inputPosition;
        }

        private void ProcessInputDrag(Vector3 inputPosition)
        {
            if (!this.isTouch)
            {
                return;
            }

            Vector2 screenDiff = this.currentPosition - inputPosition;
            Vector2 normalizedDiff = this.camera.ScreenToViewportPoint(screenDiff) * this.dragSpeed;

            if (!this.isDrag && normalizedDiff.magnitude > this.minDrag)
            {
                this.isDrag = true;
            }
            
            this.OnDrag?.Invoke(normalizedDiff);
            this.currentPosition = inputPosition;
        }

        private void ProcessInputUp()
        {
            this.isTouch = false;
            if (!this.isDrag)
            {
                return;
            }

            this.isDrag = false;
        }

        #endregion
    }
}