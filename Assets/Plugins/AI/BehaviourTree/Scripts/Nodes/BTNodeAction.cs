// using System;
// using Sirenix.OdinInspector;
// using UnityEngine;
//
// namespace Atomic.AI
// {
//     [Serializable]
//     public sealed class BTNodeAction : BTNode
//     {
//         public override string Name => this.name;
//
//         [GUIColor(1f, 0.92156863f, 0.015686275f)]
//         [SerializeField]
//         private string name;
//         
//         [SerializeReference]
//         private IBlackboardAction[] actions = default;
//         
//         protected override BTState OnUpdate(IBlackboard blackboard, float deltaTime)
//         {
//             if (this.actions != null)
//             {
//                 for (int i = 0, count = this.actions.Length; i < count; i++)
//                 {
//                     IBlackboardAction action = this.actions[i];
//                     action?.Invoke(blackboard);
//                 }
//             }
//
//             return BTState.SUCCESS;
//         }
//     }
// }