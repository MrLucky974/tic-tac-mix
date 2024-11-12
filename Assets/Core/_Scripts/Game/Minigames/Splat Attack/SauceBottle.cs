using UnityEngine;

namespace RapidPrototyping.TicTacMix.SplatAttack
{
    public class SauceBottle : MonoBehaviour
    {
        private const float SPEED = 10f;
        private const float RESPONSE = 25f;

        [SerializeField] private PlayerIdentifier m_identifier;

        [Space]

        [SerializeField] private Splat m_splatPrefab;
        [SerializeField] private SpriteRenderer m_renderer;
        [SerializeField] private Transform m_spawnPoint;

        [Header("Animation")]
        [SerializeField] private float m_spring = 300f;
        [SerializeField] private float m_damp = 25f;
        [SerializeField] private float m_scaleFactor = 2f;

        private GameInputActions m_inputActions;

        private bool m_primaryRequested;
        private Vector2 m_requestedMovement;

        private Vector2 m_velocity;

        private float m_springVelocity;
        private float m_springDisplacement;
        private Vector3 m_defaultScale;

        private void Start()
        {
            m_inputActions = InputManager.InputActions; // Grab a reference to the input

            // Set the player color based on the identifier
            var color = GameManager.GetColor(m_identifier);
            m_renderer.color = color;

            m_defaultScale = m_renderer.transform.localScale;
        }

        private void Update()
        {
            UpdateInput();

            // Update sauce bottle velocity
            if (m_requestedMovement.magnitude > 0f)
            {
                var targetVelocity = m_requestedMovement.normalized
                    * SPEED * Time.deltaTime;

                m_velocity = Vector2.Lerp(m_velocity,
                    targetVelocity,
                    1f - Mathf.Exp(-RESPONSE * Time.deltaTime)
                );
            }
            else
            {
                m_velocity = Vector2.Lerp(m_velocity,
                    Vector2.zero,
                    1f - Mathf.Exp(-RESPONSE * Time.deltaTime)
                );
            }

            // If primary action is pressed, spawn a new splat prefab
            if (m_primaryRequested)
            {
                var instance = m_splatPrefab.Create(m_identifier);
                instance.transform.position = m_spawnPoint.position;
                instance.transform.parent = GameManager.SplatContainer;
                m_springVelocity = 30f;

                m_primaryRequested = false;
            }

            // Move the bottle by velocity
            transform.Translate(m_velocity, Space.Self);

            var force = -m_spring * m_springDisplacement - m_damp * m_springVelocity;
            m_springVelocity += force * Time.deltaTime;
            m_springDisplacement += m_springVelocity * Time.deltaTime;
            m_renderer.transform.localScale = m_defaultScale + new Vector3(-m_springDisplacement, m_springDisplacement) * m_scaleFactor;
        }

        private void LateUpdate()
        {
            // Prevent players from going outside the game bounds
            transform.ClampPositionToRect(GameManager.WorldRect);
        }

        private void UpdateInput()
        {
            switch (m_identifier)
            {
                case PlayerIdentifier.PLAYER_ONE:
                    var p1Input = m_inputActions.P1Gameplay;

                    m_requestedMovement = p1Input.Movement.ReadValue<Vector2>();

                    if (p1Input.Primary.WasPressedThisFrame())
                    {
                        m_primaryRequested = true;
                    }
                    break;
                case PlayerIdentifier.PLAYER_TWO:
                    var p2Input = m_inputActions.P2Gameplay;

                    m_requestedMovement = p2Input.Movement.ReadValue<Vector2>();

                    if (p2Input.Primary.WasPressedThisFrame())
                    {
                        m_primaryRequested = true;
                    }
                    break;
            }
        }
    }
}
