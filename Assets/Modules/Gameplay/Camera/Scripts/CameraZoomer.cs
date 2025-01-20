using System;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

namespace Modules.Gameplay
{
    public sealed class CameraZoomer
    {
        [Serializable]
        public sealed class Zoom
        {
            public float fieldOfView;
            public float smoothTime;
            public int priority;
        }

        private const float MAX_BLENDING_SPEED = 1000;
        private const float SMOOTH_TIME = 0.5f;

        private readonly CinemachineVirtualCamera playerCamera;
        private readonly List<Zoom> zooms = new();

        private float _currentVelocity;

        public CameraZoomer(CinemachineVirtualCamera virtualCamera)
        {
            this.playerCamera = virtualCamera;
            this.AddZoom(new Zoom
            {
                priority = 0,
                fieldOfView = playerCamera.m_Lens.FieldOfView,
                smoothTime = SMOOTH_TIME
            });
        }
        
        public CameraZoomer(CinemachineVirtualCamera virtualCamera, Zoom baseZoom)
        {
            this.playerCamera = virtualCamera;
            this.AddZoom(baseZoom);
        }

        public bool ContainsZoom(Zoom zoom)
        {
            return this.zooms.Contains(zoom);
        }

        public bool AddZoom(Zoom zoom)
        {
            if (this.zooms.Contains(zoom))
            {
                return false;
            }

            this.zooms.Add(zoom);
            return true;
        }

        public bool RemoveZoom(Zoom zoom)
        {
            return this.zooms.Remove(zoom);
        }

        public IReadOnlyList<Zoom> GetAllZooms()
        {
            return this.zooms;
        }

        public void Update()
        {
            if (this.zooms.Count == 0)
            {
                return;
            }

            Zoom targetZoom = this.zooms[0];
            int targetPriority = targetZoom.priority;

            for (int i = 1, count = this.zooms.Count; i < count; i++)
            {
                Zoom zoom = this.zooms[i];
                if (zoom.priority > targetPriority)
                {
                    targetZoom = zoom;
                    targetPriority = zoom.priority;
                }
            }

            this.playerCamera.m_Lens.FieldOfView = Mathf.SmoothDamp(
                this.playerCamera.m_Lens.FieldOfView,
                targetZoom.fieldOfView,
                ref _currentVelocity,
                targetZoom.smoothTime,
                MAX_BLENDING_SPEED,
                Time.deltaTime
            );
        }
    }
}