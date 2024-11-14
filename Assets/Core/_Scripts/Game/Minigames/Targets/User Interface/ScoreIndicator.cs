using TMPro;
using UnityEngine;

namespace RapidPrototyping.TicTacMix
{
    public class ScoreIndicator : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private TextMeshPro m_label;

        [Header("Opacity")]
        [SerializeField] private float m_lifetime = 1f;

        [Header("Vertical Movement")]
        [SerializeField] private float m_speed = 10f;

        [Header("Horizontal Movement")]
        [SerializeField] private float m_horizontalMovementAmplitude = 0.8f;
        [SerializeField] private float m_horizontalMovementFrequency = 2f;

        private float m_horizontalMovementTime = 0f;
        private float m_remainingTime;

        private void Start()
        {
            m_remainingTime = m_lifetime;
        }

        private void Update()
        {
            if (m_remainingTime <= 0f)
            {
                Destroy(gameObject);
            }

            var deltaTime = Time.deltaTime;

            // Decrease opacity over time
            m_remainingTime -= deltaTime;

            var color = m_label.color;
            float opacity = Mathf.Lerp(1f, 0f, 1f - (m_remainingTime / m_lifetime));
            var newColor = new Color(color.r, color.g, color.b, opacity);

            m_label.color = newColor;

            // Oscillate horizontally
            m_horizontalMovementTime += deltaTime;
            var position = transform.localPosition;
            position.x = Mathf.Sin(m_horizontalMovementTime * m_horizontalMovementFrequency) * m_horizontalMovementAmplitude;
            transform.localPosition = position;

            // Move upwards with speed
            transform.Translate(transform.up * m_speed * deltaTime, Space.Self);
        }

        public ScoreIndicator Create(Vector3 position, string text)
        {
            return Create(position, text, Color.white);
        }

        public ScoreIndicator Create(Vector3 position, string text, Color color)
        {
            var instance = Instantiate(this, position, Quaternion.identity);
            instance.m_label.SetText(text);
            instance.m_label.color = color;
            return instance;
        }
    }
}
