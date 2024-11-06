using System.Collections.Generic;
using UnityEngine;

namespace RapidPrototyping.TicTacMix.MysteryDoors
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Camera m_camera;

        [Space]

        [SerializeField] private Vector3 m_offset;
        [SerializeField] private float m_minZoom = 8f;
        [SerializeField] private float m_maxZoom = 5f;
        [SerializeField] private float m_zoomLimit = 13f;

        private readonly List<Transform> m_targets = new();

        private Vector3 m_velocity;

        public void AddTarget(Transform target)
        {
            m_targets.Add(target);
        }

        public void RemoveTarget(Transform target)
        {
            if (m_targets.Contains(target) is false)
                return;

            m_targets.Remove(target);
        }

        private void LateUpdate()
        {
            if (m_targets.Count == 0)
                return;

            Move();
            Zoom();
        }

        private void Zoom()
        {
            float targetZoom = Mathf.Lerp(m_minZoom, m_maxZoom,
                GetGreatestDistance() / m_zoomLimit);
            m_camera.orthographicSize = Mathf.Lerp(m_camera.orthographicSize,
                targetZoom, 1f - Mathf.Exp(-10f * Time.deltaTime));
        }

        private void Move()
        {
            Vector3 centerPoint = GetCenterPoint();
            Vector3 targetPosition = centerPoint + m_offset;
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref m_velocity, 0.5f);
        }

        private float GetGreatestDistance()
        {
            var bounds = new Bounds(m_targets[0].position, Vector3.zero);

            for (int i = 0; i < m_targets.Count; i++)
            {
                bounds.Encapsulate(m_targets[i].position);
            }

            return Mathf.Max(bounds.size.x, bounds.size.y);
        }

        private Vector3 GetCenterPoint()
        {
            if (m_targets.Count == 1)
                return m_targets[0].position;

            var bounds = new Bounds(m_targets[0].position, Vector3.zero);
            for (int i = 0; i < m_targets.Count; i++)
            {
                bounds.Encapsulate(m_targets[i].position);
            }

            return bounds.center;
        }
    }
}
