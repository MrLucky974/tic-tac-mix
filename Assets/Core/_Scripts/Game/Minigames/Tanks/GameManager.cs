using LuckiusDev.Utils;
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
        private CountdownTimer m_timer;

        public override void Initialize()
        {
            base.Initialize();
            m_timer = new CountdownTimer(3f);
            m_timer.OnTimerStop += () =>
            {
                if (m_timer.IsFinished is false)
                    return;

                LoadGameplaySceneForNextTurn();
            };
        }

        private void Update()
        {
            var unscaledDeltaTime = Time.unscaledDeltaTime;
            m_timer.Tick(unscaledDeltaTime);
        }

        protected override void HandleGameEnd(GameData data)
        {
            base.HandleGameEnd(data);

            MarkWinningSymbol(data.PlayerIndex);
            m_timer.Start();
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
