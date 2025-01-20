// using System;
// using Sirenix.OdinInspector;
// using UnityEngine;
//
// namespace Atomic.AI
// {
//     [Serializable]
//     public abstract class BTNodeComposite : BTNode, IBTNodeParent
//     {
//         public override string Name => this.name;
//
//         [GUIColor(1f, 0.92156863f, 0.015686275f)]
//         [SerializeField]
//         private string name;
//
// #if UNITY_EDITOR
//         [ListDrawerSettings(OnBeginListElementGUI = nameof(DrawNodeLabel))]
// #endif
//         [SerializeReference, HideLabel]
//         private protected BTNode[] nodes;
//         
//         public bool FindChild(string name, out BTNode result)
//         {
//             for (int i = 0, count = this.nodes.Length; i < count; i++)
//             {
//                 BTNode node = this.nodes[i];
//                 if (name.Equals(node.Name))
//                 {
//                     result = node;
//                     return true;
//                 }
//             }
//             
//             for (int i = 0, count = this.nodes.Length; i < count; i++)
//             {
//                 if (this.nodes[i] is IBTNodeParent parent && parent.FindChild(name, out result))
//                 {
//                     return true;
//                 }
//             }
//
//             result = default;
//             return false;
//         }
//
// #if UNITY_EDITOR
//         private void DrawNodeLabel(int index)
//         {
//             if (this.nodes == null)
//             {
//                 GUILayout.Label("Undefined");
//                 return;
//             }
//
//             BTNode node = this.nodes[index];
//
//             string label = string.IsNullOrWhiteSpace(node.Name)
//                 ? $"{index + 1}. Undefined"
//                 : $"{index + 1}. {node.Name}";
//
//             GUILayout.Space(4);
//
//             Color color = GUI.color;
//             GUI.color = Color.yellow;
//             GUILayout.Label(label);
//             GUI.color = color;
//         }
// #endif
//     }
// }