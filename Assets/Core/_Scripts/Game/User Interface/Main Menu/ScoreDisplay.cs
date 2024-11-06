using LuckiusDev.Utils;
using TMPro;
using UnityEngine;

namespace RapidPrototyping.TicTacMix
{
    public class ScoreDisplay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI m_playerOneScoreLabel;
        [SerializeField] private TextMeshProUGUI m_playerTwoScoreLabel;

        private void Start()
        {
            GameDataHandler.Instance.OnScoreChanged += UpdateScores;
            UpdateScores(GameDataHandler.PlayerOneScore, GameDataHandler.PlayerTwoScore);
        }

        private void OnDestroy()
        {
            GameDataHandler.Instance.OnScoreChanged -= UpdateScores;
        }

        private void UpdateScores(int p1Score, int p2Score)
        {
            m_playerOneScoreLabel.SetText(NumberFormatter.FormatNumberWithSuffix(p1Score));
            m_playerTwoScoreLabel.SetText(NumberFormatter.FormatNumberWithSuffix(p2Score));
        }
    }
}
