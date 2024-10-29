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

        private void Start()
        {
            m_remainingTime = m_totalDuration;
        }

        private void Update()
        {
            if (m_isComplete)
                return;

            m_remainingTime -= Time.deltaTime;

            if (m_remainingTime <= 0f)
            {
                OnTimerComplete?.Invoke();
                m_isComplete = true;
            }
        }
    }
}
