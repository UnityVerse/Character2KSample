#if UNITY_EDITOR
using Sirenix.OdinInspector.Editor;
using UnityEditor;

namespace Atomic.AI
{
    [CustomEditor(typeof(SceneBlackboard))]
    public sealed class BlackboardEditor : OdinEditor
    {
        protected override void OnEnable()
        {
            base.OnEnable();
            SceneBlackboard._nameFunc = BlackboardConfig.EditorInstance.NameOf;
        }
    }
}
#endif