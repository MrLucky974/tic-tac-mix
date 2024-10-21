using UnityEngine;

namespace RapidPrototyping.TicTacMix.Tanks
{
    public class Tank : MonoBehaviour
    {
        [SerializeField] private int m_playerIndex;

        [Space]

        [SerializeField] private TankCharacter m_character;
        [SerializeField] private TankCannon m_cannon;

        private void Start()
        {
            m_cannon.Initialize();
            m_character.Initialize(m_cannon.Cannon);
        }

        private void Update()
        {
            var deltaTime = Time.deltaTime;

            if (m_playerIndex == 0)
            {
                var input = InputManager.InputActions.P1Gameplay;
                var movement = input.Movement.ReadValue<Vector2>();

                var cannonInput = new CannonInput
                {
                    Turn = movement.x,
                    Shoot = input.Primary.WasPressedThisFrame(),
                };
                m_cannon.UpdateInput(cannonInput);
                m_cannon.UpdateCannon(deltaTime);

                var characterInput = new CharacterInput
                {
                    Movement = movement.y,
                    Rotation = m_cannon.Cannon.rotation,
                };
                m_character.UpdateInput(characterInput);
                m_character.UpdateCharacter(deltaTime);
            }
            else
            {
                var input = InputManager.InputActions.P2Gameplay;
                var movement = input.Movement.ReadValue<Vector2>();

                var cannonInput = new CannonInput
                {
                    Turn = movement.x,
                    Shoot = input.Primary.WasPressedThisFrame(),
                };
                m_cannon.UpdateInput(cannonInput);
                m_cannon.UpdateCannon(deltaTime);

                var characterInput = new CharacterInput
                {
                    Movement = movement.y,
                    Rotation = m_cannon.Cannon.rotation,
                };
                m_character.UpdateInput(characterInput);
                m_character.UpdateCharacter(deltaTime);
            }
        }
    }
}
