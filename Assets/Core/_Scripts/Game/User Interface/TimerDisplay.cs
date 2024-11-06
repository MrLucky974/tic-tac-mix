using TMPro;
using UnityEngine;

namespace RapidPrototyping.TicTacMix
{
    public class TimerDisplay : MonoBehaviour
    {
        [SerializeField] private GameTimer m_timer;
        [SerializeField] private TextMeshProUGUI m_timerLabel;

        private void Update()
        {
            m_timerLabel.text = (Mathf.RoundToInt(m_timer.RemainingTime)).ToString();
        }
    }
}
