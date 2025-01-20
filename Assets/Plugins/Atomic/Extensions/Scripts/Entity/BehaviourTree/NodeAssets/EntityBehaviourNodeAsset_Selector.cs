using System;
using System.Collections.Generic;
using Atomic.AI;
using Atomic.Entities;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Atomic.Extensions
{
    [Serializable]
    public sealed class EntityBehaviourNodeAsset_Selector : 
        BehaviourNodeAsset_Selector<IEntity>,
        IEntityBehaviourNodeAsset
    {
        
#if UNITY_EDITOR
        [ListDrawerSettings(OnBeginListElementGUI = nameof(DrawNodeLabel))]
#endif
        [SerializeReference, HideLabel]
        private IEntityBehaviourNodeAsset[] children = default;

        public override IEnumerable<IBehaviourNodeAsset<IEntity>> Children => this.children;
        
#if UNITY_EDITOR
        private void DrawNodeLabel(int index)
        {
            if (this.children == null)
            {
                GUILayout.Label("Undefined");
                return;
            }

            var node = this.children[index];
            if (node == null)
            {
                GUILayout.Label("Undefined");
                return;
            }

            string label = string.IsNullOrWhiteSpace(node.Name)
                ? $"{index + 1}. Undefined"
                : $"{index + 1}. {node.Name}";

            GUILayout.Space(4);

            Color color = GUI.color;
            GUI.color = Color.yellow;
            GUILayout.Label(label);
            GUI.color = color;
        }
#endif
    }
}