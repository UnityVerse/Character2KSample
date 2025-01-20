using UnityEngine;

namespace Modules.Gameplay
{
    public static class GameCycleExtensions
    {
        public static void SubscribeAllListeners(this GameCycle it, bool includeInactive = true)
        {
            GameCycleListener[] listeners = GameObject.FindObjectsOfType<GameCycleListener>(includeInactive);
            SubscribeListeners(it, listeners);
        }

        public static void SubscribeListeners(this GameCycle it, GameCycleListener[] listeners)
        {
            for (int i = 0, count = listeners.Length; i < count; i++)
            {
                it.AddListener(listeners[i]);
            }
        }

        public static void UnsubscribeAllListeners(this GameCycle it, bool includeInactive = true)
        {
            GameCycleListener[] listeners = GameObject.FindObjectsOfType<GameCycleListener>(includeInactive);
            it.UnsubscribeListeners(listeners);
        }

        public static void UnsubscribeListeners(this GameCycle it, GameCycleListener[] listeners)
        {
            for (int i = 0, count = listeners.Length; i < count; i++)
            {
                it.DelListener(listeners[i]);
            }
        }
    }
}