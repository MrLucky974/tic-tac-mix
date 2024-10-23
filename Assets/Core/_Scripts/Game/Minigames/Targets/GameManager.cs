using LuckiusDev.Utils;

namespace RapidPrototyping.TicTacMix.Targets
{
    public class GameManager : Singleton<GameManager>
    {
        public const int PLAYER_ONE_INDEX = 0;
        public const int PLAYER_TWO_INDEX = 1;

        private int m_p1Score, m_p2Score;

        public static void AddScore(int score, int playerIndex)
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
        }

        public static void RemoveScore(int score, int playerIndex)
        {
            switch (playerIndex)
            {
                case PLAYER_ONE_INDEX:
                    Instance.m_p1Score -= score;
                    break;
                case PLAYER_TWO_INDEX:
                    Instance.m_p2Score -= score;
                    break;
            }
        }

        public int P1Score => m_p1Score;
        public int P2Score => m_p2Score;
    }
}
