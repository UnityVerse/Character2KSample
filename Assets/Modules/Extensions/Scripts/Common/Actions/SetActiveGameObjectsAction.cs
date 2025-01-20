using System;
using Atomic.Elements;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace Modules.Extensions
{
    [MovedFrom(true, "Game.Engine", "Engine", null)]
    [Serializable]
    public sealed class SetActiveGameObjectsAction : IAction
    {
        [SerializeField]
        private bool active;
    
        [SerializeField]
        private GameObject[] gameObjects;
        
        public void Invoke()
        {
            if (this.gameObjects == null)
            {
                return;
            }
        
            for (int i = 0, count = this.gameObjects.Length; i < count; i++)
            {
                GameObject gameObject = this.gameObjects[i];
                if (gameObject != null)
                {
                    gameObject.SetActive(this.active);
                }
            }
        }
    }
}