using TMPro;
using UnityEngine;

namespace RapidPrototyping.TicTacMix.Targets
{
    public class TimerDisplay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI m_timerLabel;

        private void Update()
        {
            m_timerLabel.text = (Mathf.RoundToInt(GameManager.RemainingTime)).ToString();
        }
    }
}
