using CartoonFX;
using UnityEngine;

namespace RapidPrototyping.TicTacMix.Tanks
{
    public class Tank : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private int m_playerIndex;

        [Header("References")]
        [SerializeField] private TankCharacter m_character;
        [SerializeField] private TankCannon m_cannon;
        [SerializeField] private Health m_health;

        [Header("Effects")]
        [SerializeField] private CFXR_Effect m_explosionEffect;

        private void Start()
        {
            m_cannon.Initialize();
            m_character.Initialize(m_cannon.Cannon);

            m_health.OnDeath += HandleDeath;
            GameManager.Instance.OnGameEnded += HandleGameEnd;
        }

        private void HandleGameEnd(GameData data)
        {
            enabled = false;
        }

        private void OnDestroy()
        {
            m_health.OnDeath -= HandleDeath;
        }

        private void Update()
        {
            if (GameManager.GameRunning is false)
                return;

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

        private void HandleDeath()
        {
            Instantiate(m_explosionEffect, m_character.transform.position, Quaternion.identity);

            var data = new GameData
            {
                Result = MatchResult.PLAYER_DEATH,
                PlayerIndex = m_playerIndex == GameManager.PLAYER_ONE_INDEX ? GameManager.PLAYER_TWO_INDEX : GameManager.PLAYER_ONE_INDEX,
            };
            GameManager.EndGame(data);

            Destroy(gameObject);
        }
    }
}
