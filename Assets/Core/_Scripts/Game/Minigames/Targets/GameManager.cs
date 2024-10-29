using System;

namespace RapidPrototyping.TicTacMix.Targets
{
    public struct GameData
    {
        public int PlayerIndex;
    }

    public class GameManager : MinigameManager<GameData>
    {
        private int m_p1Score, m_p2Score;

        public event Action OnScoreChanged;

        private void Start()
        {
            OnScoreChanged?.Invoke();
            OnGameEnded += HandleGameEnd;
        }

        private void HandleGameEnd(GameData data)
        {
            MarkWinningSymbol(data.PlayerIndex);
        }

        public static void StopGame()
        {
            var gameManager = ((GameManager)Instance);

            // By default, set -1 as the winner (indicating that it's a tie)
            var winIndex = TIE_INDEX;
            if (gameManager.m_p1Score > gameManager.m_p2Score) // If player one has a bigger score than player two...
            {
                winIndex = PLAYER_ONE_INDEX;
            }
            else if (gameManager.m_p2Score > gameManager.m_p1Score) // Else, if player two has a bigger score...
            {
                winIndex = PLAYER_TWO_INDEX;
            }

            var data = new GameData
            {
                PlayerIndex = winIndex,
            };
            EndGame(data); // End the game.
        }

        public static void UpdateScore(int score, int playerIndex)
        {
            var gameManager = ((GameManager)Instance);

            switch (playerIndex)
            {
                case PLAYER_ONE_INDEX:
                    gameManager.m_p1Score += score;
                    break;
                case PLAYER_TWO_INDEX:
                    gameManager.m_p2Score += score;
                    break;
            }

            gameManager.OnScoreChanged?.Invoke();
        }

        public static int P1Score => ((GameManager)Instance).m_p1Score;
        public static int P2Score => ((GameManager)Instance).m_p2Score;
    }
}
