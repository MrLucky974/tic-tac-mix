namespace RapidPrototyping.TicTacMix.Tanks
{
    public enum MatchResult
    {
        PlayerDeath,
        TimeUp
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
                Result = MatchResult.TimeUp,
                PlayerIndex = TIE_INDEX
            };

            EndGame(data);
        }
    }
}
