using LuckiusDev.Utils;
using System;
using UnityEngine;

namespace RapidPrototyping.TicTacMix.Tanks
{
    public class GameManager : Singleton<GameManager>
    {
        public enum GameEndReason
        {
            PlayerDeath,
            TimeUp
        }

        [SerializeField] private float m_totalDuration;

        public event Action<GameEndReason, int> OnGameEnded;

        private bool m_gameRunning = true;
        private float m_currentTime;

        private void Start()
        {
            m_currentTime = m_totalDuration;
        }

        private void Update()
        {
            if (m_gameRunning is false)
                return;

            m_currentTime -= Time.deltaTime;

            if (m_currentTime <= 0f)
            {
                EndGame(GameEndReason.TimeUp, -1);
            }
        }

        public static void EndGame(GameEndReason reason, int winIndex)
        {
            if (Instance.m_gameRunning is false)
                return;

            Instance.OnGameEnded?.Invoke(reason, winIndex);
            Instance.m_gameRunning = false;
        }
    }
}
