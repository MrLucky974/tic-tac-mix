#if UNITY_EDITOR
using UnityEditor;
#endif

namespace LuckiusDev.Utils.Editor
{
#if UNITY_EDITOR
    [InitializeOnLoad]

    public static class PlayModeStateManager
    {
        public static PlayModeStateChange PlayState;

        // Register an event handler when the class is initialized
        static PlayModeStateManager()
        {
            EditorApplication.playModeStateChanged += LogPlayModeState;
        }

        private static void LogPlayModeState(PlayModeStateChange state)
        {
            PlayState = state;
        }
    }
#endif
}