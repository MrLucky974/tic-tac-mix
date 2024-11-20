using CartoonFX;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

namespace RapidPrototyping.TicTacMix.Tanks
{
    public class Tank : MonoBehaviour, IPlayerControls
    {
        [Header("Settings")]
        [SerializeField] private int m_playerIndex;
        [SerializeField] private PlayerInput m_playerInput;

        [Header("References")]
        [SerializeField] private TankCharacter m_character;
        [SerializeField] private TankCannon m_cannon;
        [SerializeField] private Health m_health;

        [Header("Effects")]
        [SerializeField] private CFXR_Effect m_explosionEffect;

        private Vector2 m_movementInput;
        private bool m_primaryWasPressedThisFrame;

        private void Start()
        {
            m_playerInput.SwitchCurrentControlScheme(m_playerInput.defaultControlScheme);
            InputUser.PerformPairingWithDevice(Keyboard.current, m_playerInput.user, InputUserPairingOptions.None);
            InputUser.PerformPairingWithDevice(Mouse.current, m_playerInput.user, InputUserPairingOptions.None);
            if (m_playerIndex > 0)
            {
                if (Gamepad.all.Count >= m_playerIndex)
                {
                    var gamepad = Gamepad.all[m_playerIndex - 1];
                    InputUser.PerformPairingWithDevice(gamepad, m_playerInput.user, InputUserPairingOptions.None);
                }
            }

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

        public void OnMovement(InputAction.CallbackContext ctx)
        {
            m_movementInput = ctx.ReadValue<Vector2>();
        }

        public void OnPrimary(InputAction.CallbackContext ctx)
        {
            m_primaryWasPressedThisFrame |= ctx.action.WasPressedThisFrame();
        }

        private void Update()
        {
            if (GameManager.GameRunning is false)
                return;

            var deltaTime = Time.deltaTime;

            var cannonInput = new CannonInput
            {
                Turn = m_movementInput.x,
                Shoot = m_primaryWasPressedThisFrame,
            };
            m_cannon.UpdateInput(cannonInput);
            m_cannon.UpdateCannon(deltaTime);

            var characterInput = new CharacterInput
            {
                Movement = m_movementInput.y,
                Rotation = m_cannon.Cannon.rotation,
            };
            m_character.UpdateInput(characterInput);
            m_character.UpdateCharacter(deltaTime);
        }

        private void LateUpdate()
        {
            m_primaryWasPressedThisFrame = false;
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
