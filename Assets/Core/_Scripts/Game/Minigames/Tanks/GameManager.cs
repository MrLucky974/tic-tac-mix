using UnityEngine;

namespace RapidPrototyping.TicTacMix.Tanks
{
    public enum MatchResult
    {
        PLAYER_DEATH,
        TIMES_UP
    }

    public struct GameData
    {
        public MatchResult Result;
        public int PlayerIndex;
    }

    public class GameManager : MinigameManager<GameData>
    {
        [SerializeField] private GameTimer m_timer;
        public static GameTimer Timer => ((GameManager)Instance).m_timer;

        public void Start()
        {
            OnGameEnded += HandleGameEnd;
        }

        private void HandleGameEnd(GameData data)
        {
            MarkWinningSymbol(data.PlayerIndex);
        }

        public static void ConcludeGameOnTimeout()
        {
            var data = new GameData
            {
                Result = MatchResult.TIMES_UP,
                PlayerIndex = TIE_INDEX
            };

            EndGame(data);
        }
    }
}
