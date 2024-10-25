using LuckiusDev.Utils;
using System;

namespace RapidPrototyping.TicTacMix
{
    public class GameDataHandler : PersistentSingleton<GameDataHandler>
    {
        // Indicates who's going to try to place their symbol on the grid.
        #region Turn

        public enum Turn
        {
            PLAYER_1,
            PLAYER_2,
        }

        private Turn m_currentTurn = Turn.PLAYER_1;
        public static Turn CurrentTurn => Instance.m_currentTurn;
        public event Action<Turn> OnTurnChanged;

        public static void ChangeTurn()
        {
            Instance.m_currentTurn = Instance.m_currentTurn == Turn.PLAYER_1 ?
                Turn.PLAYER_2 : Turn.PLAYER_1;
        }

        #endregion

        // The score in this script indicates how much games were won
        // by player one and two.
        #region Score

        private int m_playerOneScore;
        public static int PlayerOneScore => Instance.m_playerOneScore;
        private int m_playerTwoScore;
        public static int PlayerTwoScore => Instance.m_playerTwoScore;

        public event Action<int, int> OnScoreChanged;

        /// <summary>
        /// Add one point to player one score.
        /// </summary>
        public static void AddP1Score()
        {
            Instance.m_playerOneScore++;
        }

        /// <summary>
        /// Add one point to player two score.
        /// </summary>
        public static void AddP2Score()
        {
            Instance.m_playerTwoScore++;
        }

        /// <summary>
        /// Set both player scores by zero.
        /// </summary>
        public static void ResetScores()
        {
            Instance.m_playerOneScore = 0;
            Instance.m_playerTwoScore = 0;
        }

        #endregion
    }
}
