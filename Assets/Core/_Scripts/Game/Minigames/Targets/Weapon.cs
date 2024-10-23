using UnityEngine;
using UnityEngine.UI;

namespace RapidPrototyping.TicTacMix.Targets
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField] private int m_playerIndex;

        [Space]

        [SerializeField] private float m_speed = 10f;
        [SerializeField] private float m_response = 25f;

        [Space]

        [SerializeField] private Transform m_muzzle;
        [SerializeField] private Projectile m_projectilePrefab;
        [SerializeField] private Image m_cursor;
        [SerializeField] private Canvas m_canvas;

#if UNITY_EDITOR
        [Space]
        [SerializeField] private Transform m_debugSphere;
#endif

        private Camera m_camera;
        private Vector3 m_velocity;

        private void Start()
        {
            m_camera = Camera.main;
            UpdateWeapon();
        }

        private void Update()
        {
            var deltaTime = Time.deltaTime;

            if (m_playerIndex == 0)
            {
                var input = InputManager.InputActions.P1Gameplay;
                var movement = input.Movement.ReadValue<Vector2>();

                var targetVelocity = new Vector3(movement.x, movement.y, 0f)
                    * m_speed * deltaTime;

                m_velocity = Vector3.Lerp
                (
                    m_velocity,
                    targetVelocity,
                    1f - Mathf.Exp(-m_response * deltaTime)
                );

                if (input.Primary.WasPressedThisFrame())
                {
                    var projectile = Instantiate(m_projectilePrefab, m_muzzle.position, Quaternion.identity);

                    Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(m_canvas.worldCamera, m_cursor.rectTransform.position);
                    Ray ray = m_camera.ScreenPointToRay(screenPoint);
                    if (Physics.Raycast(ray, out var hit))
                    {
                        projectile.StartMovement(hit.point);
                    }
                }
            }
            else if (m_playerIndex == 1)
            {
                var input = InputManager.InputActions.P2Gameplay;
                var movement = input.Movement.ReadValue<Vector2>();

                var targetVelocity = new Vector3(movement.x, movement.y, 0f)
                    * m_speed * deltaTime;

                m_velocity = Vector3.Lerp
                (
                    m_velocity,
                    targetVelocity,
                    1f - Mathf.Exp(-m_response * deltaTime)
                );
            }

            UpdateWeapon();
            m_cursor.rectTransform.position += m_velocity;
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
    }
}
