using UnityEngine;

namespace RapidPrototyping.TicTacMix.Targets
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float m_moveSpeed = 5f;
        [SerializeField] private float m_arcHeight = 2f;

        private Vector3 m_startPosition;
        private Vector3 m_endPosition;
        private float m_journeyLength;
        private float m_startTime;
        private bool m_isMoving = false;

        public void StartMovement(Vector3 targetPosition)
        {
            m_startPosition = transform.position;
            m_endPosition = targetPosition;
            m_journeyLength = Vector3.Distance(m_startPosition, m_endPosition);
            m_startTime = Time.time;
            m_isMoving = true;
        }

        private void Update()
        {
            if (!m_isMoving) return;

            // Calculate how far we've moved between 0 and 1
            float distanceCovered = (Time.time - m_startTime) * m_moveSpeed;
            float fractionOfJourney = distanceCovered / m_journeyLength;

            // Clamp the fraction between 0 and 1
            fractionOfJourney = Mathf.Clamp01(fractionOfJourney);

            // Calculate the current position using linear interpolation
            Vector3 currentPosition = Vector3.Lerp(m_startPosition, m_endPosition, fractionOfJourney);

            // Add a parabolic arc using sin curve
            float arc = m_arcHeight * Mathf.Sin(fractionOfJourney * Mathf.PI);
            currentPosition.y += arc;

            // Update the ball's position
            transform.position = currentPosition;

            // Check if we've reached the destination
            if (fractionOfJourney >= 1.0f)
            {
                m_isMoving = false;
            }
        }
    }
}
