namespace RapidPrototyping.TicTacMix.MysteryDoors
{
    public class ExitDoor : Door
    {
        protected override void UseDoor(Player player)
        {
            var data = new GameData
            {
                Result = MatchResult.EXIT_DOOR_OPENED,
                PlayerIndex = player.PlayerIndex,
            };
            GameManager.EndGame(data);
        }
    }
}
