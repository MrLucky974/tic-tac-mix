using UnityEngine;

namespace RapidPrototyping.TicTacMix.MysteryDoors
{
    public class Door : MonoBehaviour
    {
        private Player m_playerOne, m_playerTwo;
        private bool m_isTrapped;

        public void Initialize(bool trapped)
        {
            m_isTrapped = trapped;
        }

        private void Update()
        {
            if (!m_playerOne && !m_playerTwo)
                return;

            if (m_playerOne)
            {
                var input = InputManager.InputActions.P1Gameplay;
                var movement = input.Movement.ReadValue<Vector2>().y;
                if (input.Movement.WasPressedThisFrame() && movement >= 1f)
                {
                    if (m_playerOne.currentStage is Floor floor)
                    {
                        var nextStage = floor.previousStage;
                        m_playerOne.SetNewStage(nextStage);
                    }
                }
            }

            if (m_playerTwo)
            {
                var input = InputManager.InputActions.P2Gameplay;
                var movement = input.Movement.ReadValue<Vector2>().y;
                if (input.Movement.WasPressedThisFrame() && movement >= 1f)
                {
                    if (m_playerTwo.currentStage is Floor floor)
                    {
                        var nextStage = floor.previousStage;
                        m_playerTwo.SetNewStage(nextStage);
                    }
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent<Player>(out var player))
            {
                if (player.PlayerIndex == 0)
                {
                    m_playerOne = player;
                }

                if (player.PlayerIndex == 1)
                {
                    m_playerTwo = player;
                }
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.TryGetComponent<Player>(out var player))
            {
                if (player.PlayerIndex == 0)
                {
                    m_playerOne = null;
                }

                if (player.PlayerIndex == 1)
                {
                    m_playerTwo = null;
                }
            }
        }
    }
}
