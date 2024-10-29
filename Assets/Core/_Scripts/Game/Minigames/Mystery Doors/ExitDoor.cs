using UnityEngine;

namespace RapidPrototyping.TicTacMix.MysteryDoors
{
    public class ExitDoor : Door
    {
        protected override void Update()
        {
            if (!m_playerOne && !m_playerTwo)
                return;

            if (m_playerOne)
            {
                var input = InputManager.InputActions.P1Gameplay;
                var movement = input.Movement.ReadValue<Vector2>().y;
                if (input.Movement.WasPressedThisFrame() && movement > 0f)
                {
                    var data = new GameData
                    {
                        Result = MatchResult.ExitDoorOpened,
                        PlayerIndex = 0,
                    };
                    GameManager.EndGame(data);
                }
            }

            if (m_playerTwo)
            {
                var input = InputManager.InputActions.P1Gameplay;
                var movement = input.Movement.ReadValue<Vector2>().y;
                if (input.Movement.WasPressedThisFrame() && movement > 0f)
                {
                    var data = new GameData
                    {
                        Result = MatchResult.ExitDoorOpened,
                        PlayerIndex = 1,
                    };
                    GameManager.EndGame(data);
                }
            }
        }
    }
}
