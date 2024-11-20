using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

namespace RapidPrototyping.TicTacMix.SplatAttack
{
    public class SauceBottle : MonoBehaviour, IPlayerControls
    {
        private const float SPEED = 10f;
        private const float RESPONSE = 25f;

        [Header("Settings")]
        [SerializeField] private PlayerIdentifier m_identifier;
        [SerializeField] private PlayerInput m_playerInput;

        [Header("References")]
        [SerializeField] private Splat m_splatPrefab;
        [SerializeField] private SpriteRenderer m_renderer;
        [SerializeField] private Transform m_spawnPoint;

        [Header("Animation")]
        [SerializeField] private float m_spring = 300f;
        [SerializeField] private float m_damp = 25f;
        [SerializeField] private float m_scaleFactor = 2f;

        [Header("Audio")]
        [SerializeField] private AudioClip[] m_splatSounds;

        private Vector2 m_velocity;

        private float m_springVelocity;
        private float m_springDisplacement;
        private Vector3 m_defaultScale;

        #region Input Variables

        private Vector2 m_movementInput;
        private bool m_primaryPressedThisFrame;

        #endregion

        private void Start()
        {
            m_playerInput.SwitchCurrentControlScheme(m_playerInput.defaultControlScheme);
            InputUser.PerformPairingWithDevice(Keyboard.current, m_playerInput.user, InputUserPairingOptions.None);
            InputUser.PerformPairingWithDevice(Mouse.current, m_playerInput.user, InputUserPairingOptions.None);
            if (m_identifier == PlayerIdentifier.PLAYER_TWO)
            {
                if (Gamepad.all.Count >= 1)
                {
                    var gamepad = Gamepad.all[0];
                    InputUser.PerformPairingWithDevice(gamepad, m_playerInput.user, InputUserPairingOptions.None);
                }
            }

            // Set the player color based on the identifier
            var color = GameManager.GetColor(m_identifier);
            m_renderer.color = color;

            m_defaultScale = m_renderer.transform.localScale;
        }

        private void Update()
        {
            UpdateDampedOscillator();

            // Update sauce bottle velocity
            if (m_movementInput.magnitude > 0f)
            {
                var targetVelocity = m_movementInput.normalized
                    * SPEED * Time.deltaTime;

                m_velocity = Vector2.Lerp(m_velocity,
                    targetVelocity,
                    1f - Mathf.Exp(-RESPONSE * Time.unscaledDeltaTime)
                );
            }
            else
            {
                m_velocity = Vector2.Lerp(m_velocity,
                    Vector2.zero,
                    1f - Mathf.Exp(-RESPONSE * Time.unscaledDeltaTime)
                );
            }

            // Move the bottle by velocity
            transform.Translate(m_velocity, Space.Self);

            if (GameManager.GameRunning is false)
                return;

            // If primary action is pressed, spawn a new splat prefab
            if (m_primaryPressedThisFrame)
            {
                var instance = m_splatPrefab.Create(m_identifier);
                instance.transform.position = m_spawnPoint.position;
                instance.transform.parent = GameManager.SplatContainer;
                m_springVelocity = 30f;

                if (m_splatSounds != null && m_splatSounds.Length > 0)
                {
                    var sound = m_splatSounds.PickRandomUnity();
                    SoundManager.Play(sound);
                }
            }
        }

        private void LateUpdate()
        {
            // Prevent players from going outside the game bounds
            transform.ClampPositionToRect(GameManager.WorldRect);

            m_primaryPressedThisFrame = false;
        }

        private void UpdateDampedOscillator()
        {
            var force = -m_spring * m_springDisplacement - m_damp * m_springVelocity;
            m_springVelocity += force * Time.unscaledDeltaTime;
            m_springDisplacement += m_springVelocity * Time.unscaledDeltaTime;
            m_renderer.transform.localScale = m_defaultScale + new Vector3(-m_springDisplacement, m_springDisplacement) * m_scaleFactor;
        }

        public void OnMovement(UnityEngine.InputSystem.InputAction.CallbackContext ctx)
        {
            m_movementInput = ctx.ReadValue<Vector2>();
        }

        public void OnPrimary(UnityEngine.InputSystem.InputAction.CallbackContext ctx)
        {
            m_primaryPressedThisFrame |= ctx.action.WasPressedThisFrame();
        }
    }
}
