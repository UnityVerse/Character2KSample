using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Modules.Gameplay
{
   public sealed class JoystickView : MonoBehaviour,
        IPointerDownHandler,
        IDragHandler,
        IPointerUpHandler
    {
        public event Action OnPressDown;
        public event Action OnPressUp;

        public bool IsPressing { get; private set; }

        public bool IsEnable
        {
            get => this.enabled;
            set
            {
                this.enabled = value;

                if (!value)
                {
                    input = Vector2.zero;
                    handle.anchoredPosition = Vector2.zero;
                    this.IsPressing = false;
                    this.OnPressUp?.Invoke();
                }
            }
        }

        public float Horizontal
        {
            get { return snapX ? SnapFloat(input.x, AxisOptions.Horizontal) : input.x; }
        }

        public float Vertical
        {
            get { return snapY ? SnapFloat(input.y, AxisOptions.Vertical) : input.y; }
        }

        public Vector2 Direction
        {
            get { return new Vector2(Horizontal, Vertical); }
        }

        [SerializeField]
        private float handleRange = 1;

        [SerializeField]
        private float deadZone = 0;

        [SerializeField]
        private AxisOptions axisOptions = AxisOptions.Both;

        [SerializeField]
        private bool snapX = false;

        [SerializeField]
        private bool snapY = false;

        [SerializeField]
        public RectTransform background;

        [SerializeField]
        private RectTransform handle;

        private Canvas canvas;
        private Camera cam;

        private Vector2 input = Vector2.zero;

        private void Start()
        {
            canvas = GetComponentInParent<Canvas>();
            if (canvas == null)
                Debug.LogError("The Joystick is not placed inside a canvas");

            Vector2 center = new Vector2(0.5f, 0.5f);
            background.pivot = center;
            handle.anchorMin = center;
            handle.anchorMax = center;
            handle.pivot = center;
            handle.anchoredPosition = Vector2.zero;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            this.IsPressing = true;
            this.OnPressDown?.Invoke();
            this.OnDrag(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            cam = null;
            if (canvas.renderMode == RenderMode.ScreenSpaceCamera)
                cam = canvas.worldCamera;

            Vector2 position = RectTransformUtility.WorldToScreenPoint(cam, background.position);
            Vector2 radius = background.sizeDelta / 2;
            input = (eventData.position - position) / (radius * canvas.scaleFactor);
            FormatInput();
            HandleInput(input.magnitude, input.normalized);
            handle.anchoredPosition = input * radius * handleRange;
        }

        private void HandleInput(float magnitude, Vector2 normalised)
        {
            if (magnitude > deadZone)
            {
                if (magnitude > 1)
                    input = normalised;
            }
            else
                input = Vector2.zero;
        }

        private void FormatInput()
        {
            input = axisOptions switch
            {
                AxisOptions.Horizontal => new Vector2(input.x, 0f),
                AxisOptions.Vertical => new Vector2(0f, input.y),
                _ => input
            };
        }

        private float SnapFloat(float value, AxisOptions snapAxis)
        {
            if (value == 0)
                return value;

            if (axisOptions == AxisOptions.Both)
            {
                float angle = Vector2.Angle(input, Vector2.up);
                return snapAxis switch
                {
                    AxisOptions.Horizontal when angle is < 22.5f or > 157.5f => 0,
                    AxisOptions.Horizontal => value > 0 ? 1 : -1,
                    AxisOptions.Vertical when angle is > 67.5f and < 112.5f => 0,
                    AxisOptions.Vertical => value > 0 ? 1 : -1,
                    _ => value
                };
            }

            return value switch
            {
                > 0 => 1,
                < 0 => -1,
                _ => 0
            };
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            input = Vector2.zero;
            handle.anchoredPosition = Vector2.zero;
            this.IsPressing = false;
            this.OnPressUp?.Invoke();
        }
        
        public enum AxisOptions
        {
            Both,
            Horizontal,
            Vertical
        }
    }
}