using TMPro;
using UnityEngine;

namespace RapidPrototyping.TicTacMix.Targets
{
    public class ScoreDisplay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI m_playerOneLabel;
        [SerializeField] private TextMeshProUGUI m_playerTwoLabel;

        private void Start()
        {
            ((GameManager)GameManager.Instance).OnScoreChanged += HandleScore;
        }

        private void HandleScore()
        {
            m_playerOneLabel.text = GameManager.P1Score.ToString();
            m_playerTwoLabel.text = GameManager.P2Score.ToString();
        }
    }
}
