// using System;
// using UnityEngine;
//
// namespace Atomic.AI
// {
//     [Serializable]
//     public sealed class BTNodeDecorator : BTNode, IBTNodeParent
//     {
//         public override string Name => this.node != null ? this.node.Name : "Undefined";
//
//         [SerializeReference, Space]
//         private BTNode node;
//
//         [Header("Actions"), Space]
//         [SerializeReference]
//         private IBlackboardAction[] enableActions = default;
//
//         [SerializeReference]
//         private IBlackboardAction[] disableActions = default;
//
//         [SerializeReference]
//         private IBlackboardAction[] updateActions = default;
//
//         [SerializeReference]
//         private IBlackboardAction[] abortActions = default;
//
//         protected override void OnEnable(IBlackboard blackboard)
//         {
//             if (this.enableActions != null)
//             {
//                 for (int i = 0, count = this.enableActions.Length; i < count; i++)
//                 {
//                     IBlackboardAction action = this.enableActions[i];
//                     action?.Invoke(blackboard);
//                 }
//             }
//         }
//
//         protected override void OnDisable(IBlackboard blackboard)
//         {
//             if (this.disableActions != null)
//             {
//                 for (int i = 0, count = this.disableActions.Length; i < count; i++)
//                 {
//                     IBlackboardAction action = this.disableActions[i];
//                     action?.Invoke(blackboard);
//                 }
//             }
//         }
//
//         protected override BTState OnUpdate(IBlackboard blackboard, float deltaTime)
//         {
//             if (this.updateActions != null)
//             {
//                 for (int i = 0, count = this.updateActions.Length; i < count; i++)
//                 {
//                     IBlackboardAction action = this.updateActions[i];
//                     action?.Invoke(blackboard);
//                 }
//             }
//             
//             return this.node.Run(blackboard, deltaTime);
//         }
//
//         protected override void OnAbort(IBlackboard blackboard)
//         {
//             if (this.abortActions != null)
//             {
//                 for (int i = 0, count = this.abortActions.Length; i < count; i++)
//                 {
//                     IBlackboardAction action = this.abortActions[i];
//                     action?.Invoke(blackboard);
//                 }
//             }
//             
//             this.node.Abort(blackboard);
//         }
//
//         public bool FindChild(string name, out BTNode result)
//         {
//             if (name.Equals(this.node.Name))
//             {
//                 result = this.node;
//                 return true;
//             }
//
//             if (this.node is IBTNodeParent parent && parent.FindChild(name, out result))
//             {
//                 return true;
//             }
//
//             result = default;
//             return false;
//         }
//     }
// }