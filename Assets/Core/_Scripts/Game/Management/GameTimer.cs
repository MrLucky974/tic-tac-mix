using UnityEngine;
using UnityEngine.Events;

namespace RapidPrototyping.TicTacMix
{
    /// <summary>
    /// A MonoBehaviour class used by game managers to end the game.
    /// </summary>
    public class GameTimer : MonoBehaviour
    {
        [SerializeField] private float m_totalDuration;

        public UnityEvent OnTimerComplete;

        private float m_remainingTime;
        public float RemainingTime => m_remainingTime;

        private bool m_isComplete;
        private bool m_isStopped;

        private void Start()
        {
            m_remainingTime = m_totalDuration;
        }

        private void Update()
        {
            if (m_isComplete)
                return;

            if (m_isStopped)
                return;

            m_remainingTime -= Time.deltaTime;

            if (m_remainingTime <= 0f)
            {
                OnTimerComplete?.Invoke();
                m_isComplete = true;
            }
        }

        public void ResetTimer()
        {
            m_remainingTime = m_totalDuration;
            m_isStopped = false;
            m_isComplete = false;
        }

        public void Resume()
        {
            if (m_isComplete)
                return;

            if (m_isStopped is false)
                return;

            m_isStopped = false;
        }

        public void Stop()
        {
            if (m_isComplete)
                return;

            if (m_isStopped)
                return;

            m_isStopped = true;
        }
    }
}
