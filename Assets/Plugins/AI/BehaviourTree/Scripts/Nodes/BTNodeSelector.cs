// using System;
// using Sirenix.OdinInspector;
//
// namespace Atomic.AI
// {
//     [Serializable]
//     public sealed class BTNodeSelector : BTNodeComposite
//     {
//         [ShowInInspector, ReadOnly, HideInEditorMode]
//         private int nodeIndex;
//         
//         protected override void OnEnable(IBlackboard blackboard)
//         {
//             this.nodeIndex = 0;
//         }
//         
//         protected override void OnAbort(IBlackboard blackboard)
//         {
//             BTNode currentNode = this.nodes[this.nodeIndex];
//             currentNode.Abort(blackboard);
//         }
//         
//         protected override BTState OnUpdate(IBlackboard blackboard, float deltaTime)
//         {
//             BTNode currentNode = this.nodes[this.nodeIndex];
//             BTState result = currentNode.Run(blackboard, deltaTime);
//             
//             if (result != BTState.FAILURE)
//             {
//                 return result;
//             }
//
//             //Failure:
//             if (this.nodeIndex == this.nodes.Length - 1)
//             {
//                 return BTState.FAILURE;
//             }
//
//             this.nodeIndex++;
//             return BTState.RUNNING;
//         }
//     }
// }