using RapidPrototyping.Utils.Input;
using UnityEngine;
using UnityEngine.UI;

namespace RapidPrototyping.TicTacMix.Targets
{
    public class Weapon : MonoBehaviour, IPlayerControls
    {
        [Header("Settings")]
        [SerializeField] private int m_playerIndex;

        [Space]

        [SerializeField] private float m_speed = 10f;
        [SerializeField] private float m_response = 25f;

        [Header("References")]
        [SerializeField] private LayerMask m_projectileMask;
        [SerializeField] private Transform m_muzzle;
        [SerializeField] private Projectile m_projectilePrefab;
        [SerializeField] private Image m_cursor;
        [SerializeField] private Canvas m_canvas;

        [Header("Audio")]
        [SerializeField] private AudioClip[] m_shootSounds;

#if UNITY_EDITOR
        [Space]
        [SerializeField] private Transform m_debugSphere;
#endif

        private Camera m_camera;
        private Vector3 m_velocity;

        #region Input Variables

        private Vector2 m_movementInput;
        private bool m_primaryPressedThisFrame;

        #endregion

        private void Start()
        {
            m_camera = Camera.main;
            UpdateWeapon();
            GameManager.Instance.OnGameEnded += HandleGameEnded;

            GameInputHandler.SetReciever(gameObject, m_playerIndex);
        }

        private void HandleGameEnded(GameData data)
        {
            enabled = false;
        }

        private void Update()
        {
            if (GameManager.GameRunning is false)
                return;

            var deltaTime = Time.deltaTime;

            var movement = m_movementInput;

            var targetVelocity = new Vector3(movement.x, movement.y, 0f)
                * m_speed * deltaTime;

            m_velocity = Vector3.Lerp
            (
                m_velocity,
                targetVelocity,
                1f - Mathf.Exp(-m_response * deltaTime)
            );

            if (m_primaryPressedThisFrame)
            {
                var projectile = Instantiate(m_projectilePrefab, m_muzzle.position, Quaternion.identity);
                projectile.Initialize(m_playerIndex);

                if (m_shootSounds != null && m_shootSounds.Length > 0)
                {
                    var sound = m_shootSounds.PickRandomUnity();
                    SoundManager.Play(sound);
                }

                Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(m_canvas.worldCamera, m_cursor.rectTransform.position);
                Ray ray = m_camera.ScreenPointToRay(screenPoint);

                if (Physics.Raycast(ray, out var hit, 10f, m_projectileMask))
                {
                    var direction = (hit.point - m_muzzle.position).normalized;
                    projectile.Fire(direction);
                }
                else
                {
                    projectile.Fire(ray.direction);
                }
            }

            UpdateWeapon();
            m_cursor.rectTransform.position += m_velocity;
        }

        private void LateUpdate()
        {
            m_primaryPressedThisFrame = false;
        }

        private void UpdateWeapon()
        {
            var cursorPosition = GetCursorPosition();

#if UNITY_EDITOR
            if (m_debugSphere)
            {
                m_debugSphere.position = cursorPosition;
            }
#endif

            transform.forward = (cursorPosition - transform.position).normalized;
        }

        private Vector3 GetCursorPosition()
        {
            Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(m_canvas.worldCamera, m_cursor.rectTransform.position);
            Ray ray = m_camera.ScreenPointToRay(screenPoint);
            return ray.origin + ray.direction * 10f;
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
