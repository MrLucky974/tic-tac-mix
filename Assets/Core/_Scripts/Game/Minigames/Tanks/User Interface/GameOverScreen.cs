using TMPro;
using UnityEngine;

namespace RapidPrototyping.TicTacMix.Tanks
{
    public class GameOverScreen : MonoBehaviour
    {
        [SerializeField] private GameObject m_panel;

        [SerializeField] private TextMeshProUGUI m_header;
        [SerializeField] private TextMeshProUGUI m_winnerLabel;

        private void Start()
        {
            m_panel.SetActive(false);
            GameManager.Instance.OnGameEnded += HandleEndGame;
        }

        private void HandleEndGame(GameManager.GameEndReason reason, int winIndex)
        {
            m_panel.SetActive(true);

            switch (reason)
            {
                case GameManager.GameEndReason.PlayerDeath:
                    m_header.SetText("Game Over");
                    break;
                case GameManager.GameEndReason.TimeUp:
                    m_header.SetText("Times up!");
                    break;
            }

            if (winIndex != -1)
            {
                var text = winIndex == 1 ? "<color=#6982FF>Player X wins!</color>" : "<color=#FF6A6E>Player O wins!</color>";
                m_winnerLabel.SetText(text);
            }
            else
            {
                m_winnerLabel.SetText("It's a tie :(");
            }
        }
    }
}
