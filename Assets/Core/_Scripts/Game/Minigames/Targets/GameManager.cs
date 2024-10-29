using LuckiusDev.Utils;
using System;
using UnityEngine.SceneManagement;

namespace RapidPrototyping.TicTacMix.Targets
{
    public class GameManager : Singleton<GameManager>
    {
        public const int TIE_INDEX = -1;
        public const int PLAYER_ONE_INDEX = 0;
        public const int PLAYER_TWO_INDEX = 1;

        private bool m_gameRunning = true;
        public static bool GameRunning => Instance.m_gameRunning;

        private int m_p1Score, m_p2Score;

        public event Action OnScoreChanged;
        public event Action<int> OnGameEnded;

        private void Start()
        {
            OnScoreChanged?.Invoke();
        }

        public static void StopGame()
        {
            // By default, set -1 as the winner (indicating that it's a tie)
            var winIndex = TIE_INDEX;
            if (Instance.m_p1Score > Instance.m_p2Score) // If player one has a bigger score than player two...
            {
                winIndex = PLAYER_ONE_INDEX;
            }
            else if (Instance.m_p2Score > Instance.m_p1Score) // Else, if player two has a bigger score...
            {
                winIndex = PLAYER_TWO_INDEX;
            }

            EndGame(winIndex); // End the game.
        }

        private static void EndGame(int winIndex)
        {
            if (Instance.m_gameRunning is false)
                return;

            Instance.OnGameEnded?.Invoke(winIndex);
            Instance.m_gameRunning = false;

            GameDataHandler.ChangeTurn();
            SceneManager.LoadScene(GameDataHandler.MainGameplaySceneReference);
        }

        public static void UpdateScore(int score, int playerIndex)
        {
            switch (playerIndex)
            {
                case PLAYER_ONE_INDEX:
                    Instance.m_p1Score += score;
                    break;
                case PLAYER_TWO_INDEX:
                    Instance.m_p2Score += score;
                    break;
            }

            Instance.OnScoreChanged?.Invoke();
        }

        public static int P1Score => Instance.m_p1Score;
        public static int P2Score => Instance.m_p2Score;
    }
}
