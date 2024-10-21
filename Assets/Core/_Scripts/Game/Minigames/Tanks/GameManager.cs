using LuckiusDev.Utils;
using System;

namespace RapidPrototyping.TicTacMix.Tanks
{
    public class GameManager : Singleton<GameManager>
    {
        public event Action OnGameEnded;

        private bool m_gameRunning = true;

        public static void EndGame()
        {
            if (Instance.m_gameRunning is false)
                return;

            Instance.OnGameEnded?.Invoke();
            Instance.m_gameRunning = false;
        }
    }
}
