using TMPro;
using UnityEngine;

namespace RapidPrototyping.TicTacMix.SplatAttack
{
    public class GameOverScreen : MonoBehaviour
    {
        [SerializeField] private CanvasGroup m_canvasGroup;

        [Space]

        [SerializeField] private TextMeshProUGUI m_winnerLabel;

        private void Start()
        {
            m_canvasGroup.alpha = 0f;
            m_canvasGroup.interactable = false;
            m_canvasGroup.GetComponent<RectTransform>().localScale = Vector3.zero;

            GameManager.Instance.OnGameEnded += HandleEndGame;

        }

        private void HandleEndGame(GameData data)
        {
            m_canvasGroup.alpha = 1f;
            m_canvasGroup.interactable = true;
            m_canvasGroup.GetComponent<RectTransform>().localScale = Vector3.one;

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
