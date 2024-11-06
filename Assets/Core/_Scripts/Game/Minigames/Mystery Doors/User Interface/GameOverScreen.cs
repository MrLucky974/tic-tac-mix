using LuckiusDev.Utils;
using TMPro;
using UnityEngine;

namespace RapidPrototyping.TicTacMix.MysteryDoors
{
    public class GameOverScreen : MonoBehaviour
    {
        [SerializeField] private GameObject m_panel;

        [SerializeField] private TextMeshProUGUI m_header;
        [SerializeField] private TextMeshProUGUI m_winnerLabel;

        private CountdownTimer m_timer;

        private void Start()
        {
            m_panel.SetActive(false);
            GameManager.Instance.OnGameEnded += HandleEndGame;
            m_timer = new CountdownTimer(1f);
            m_timer.OnTimerStop += () =>
            {
                if (m_timer.IsFinished is false)
                    return;

                GameManager.LoadGameplaySceneForNextTurn();
            };
        }

        private void Update()
        {
            var unscaledDeltaTime = Time.unscaledDeltaTime;
            m_timer.Tick(unscaledDeltaTime);
        }

        private void HandleEndGame(GameData data)
        {
            m_panel.SetActive(true);

            switch (data.Result)
            {
                case MatchResult.EXIT_DOOR_OPENED:
                    m_header.SetText("Game Over");
                    break;
                case MatchResult.TIMES_UP:
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

            m_timer.Start();
        }
    }
}
