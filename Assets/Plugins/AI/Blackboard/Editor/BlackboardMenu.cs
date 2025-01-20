#if UNITY_EDITOR
using UnityEditor;

namespace Atomic.AI
{
    internal static class BlackboardMenu
    {
        [MenuItem("Window/Atomic/AI/Blackboard", priority = 7)]
        internal static void ShowWindow()
        {
            EditorWindow.GetWindow(typeof(BlackboardWindow));
        }

        [MenuItem("Tools/Atomic/AI/Select Blackboard Config", priority = 7)]
        internal static void SelectConfig()
        {
            Selection.activeObject = BlackboardConfig.EditorInstance;
        }
    }
}
#endif