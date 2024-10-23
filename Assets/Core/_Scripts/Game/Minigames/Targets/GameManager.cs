using LuckiusDev.Utils;
using System;

namespace RapidPrototyping.TicTacMix.Targets
{
    public class GameManager : Singleton<GameManager>
    {
        public const int PLAYER_ONE_INDEX = 0;
        public const int PLAYER_TWO_INDEX = 1;

        private int m_p1Score, m_p2Score;

        public event Action OnScoreChanged;

        private void Start()
        {
            OnScoreChanged?.Invoke();
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
