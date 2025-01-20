// using System;
// using Sirenix.OdinInspector;
//
// namespace Atomic.AI
// {
//     [Serializable]
//     public sealed class BTNodeSequence : BTNodeComposite
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
//             if (result != BTState.SUCCESS)
//             {
//                 return result;
//             }
//
//             //Success:
//             if (this.nodeIndex == this.nodes.Length - 1)
//             {
//                 return BTState.SUCCESS;
//             }
//
//             this.nodeIndex++;
//             return BTState.RUNNING;
//         }
//     }
// }