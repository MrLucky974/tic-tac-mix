using UnityEngine;

namespace LuckiusDev.Utils
{
    public static class GameTime
    {
        static GameTime()
        {
            Application.quitting += OnApplicationQuit;
        }

        private static void OnApplicationQuit()
        {
            LocalTimeScale = 1f;
        }
        
        public static float LocalTimeScale = 1f;
        public static float DeltaTime => Time.deltaTime * LocalTimeScale;
        public static float FixedDeltaTime => Time.fixedDeltaTime * LocalTimeScale;
        public static float TimeScale => Time.timeScale * LocalTimeScale;
        public static float FixedTimeScale => Time.fixedDeltaTime * LocalTimeScale;
    }
}