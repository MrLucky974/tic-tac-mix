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

        private void HandleEndGame(GameData data)
        {
            m_panel.SetActive(true);

            switch (data.Result)
            {
                case MatchResult.PlayerDeath:
                    m_header.SetText("Game Over");
                    break;
                case MatchResult.TimeUp:
                    m_header.SetText("Times up!");
                    break;
            }

            if (data.PlayerIndex != GameManager.TIE_INDEX)
            {
                var text = data.PlayerIndex == GameManager.PLAYER_ONE_INDEX ? "<color=#6982FF>Player X wins!</color>" : "<color=#FF6A6E>Player O wins!</color>";
                m_winnerLabel.SetText(text);
            }
            else
            {
                m_winnerLabel.SetText("It's a tie :(");
            }
        }
    }
}
