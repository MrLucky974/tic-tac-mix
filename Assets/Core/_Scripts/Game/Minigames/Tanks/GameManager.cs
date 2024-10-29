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
