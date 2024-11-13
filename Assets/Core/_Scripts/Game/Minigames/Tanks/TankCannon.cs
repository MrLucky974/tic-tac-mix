using CartoonFX;
using UnityEngine;

namespace RapidPrototyping.TicTacMix.Tanks
{
    public struct CannonInput
    {
        public float Turn;
        public bool Shoot;
    }

    public class TankCannon : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private Transform m_cannon;
        [SerializeField] private Transform m_muzzle;
        [SerializeField] private Missile m_missilePrefab;
        [SerializeField] private float m_delay = 0.2f;

        [Header("Effects")]

        [SerializeField] private CFXR_Effect m_shootEffect;
        [SerializeField] private LineRenderer m_lineRenderer;
        [SerializeField] private LayerMask m_wallMask;

        [Header("Audio")]
        [SerializeField] private AudioClip m_shootSound;

        public Transform Cannon => m_cannon;

        private Quaternion m_requestedRotation;
        private bool m_requestedShoot;
        private float m_nextShot;

        public void Initialize()
        {
            m_requestedRotation = m_cannon.rotation;
        }

        public void UpdateInput(CannonInput input)
        {
            var rotation = Quaternion.AngleAxis(input.Turn * 0.2f, Vector3.up);
            m_requestedRotation *= rotation;

            m_requestedShoot = m_requestedShoot || input.Shoot;
        }

        public void UpdateCannon(float deltaTime)
        {
            UpdateRotation(deltaTime);

            if (m_requestedShoot && Time.time > m_nextShot)
            {
                Shoot();
                m_requestedShoot = false;
                m_nextShot = Time.time + m_delay;
            }

            Vector3 targetPosition = Vector3.zero;
            Ray ray = new Ray(m_muzzle.position, m_muzzle.forward);
            if (Physics.Raycast(ray, out var hit, 1000f, m_wallMask))
            {
                targetPosition = m_muzzle.InverseTransformPoint(hit.point);
            }
            else
            {
                targetPosition = m_muzzle.forward * 10f;
            }
            m_lineRenderer.SetPosition(1, targetPosition);
        }

        public void UpdateRotation(float deltaTime)
        {
            var currentRotation = m_cannon.transform.rotation;
            currentRotation = Quaternion.Lerp(currentRotation, m_requestedRotation, 1f - Mathf.Exp(-25f * deltaTime));
            m_cannon.transform.rotation = currentRotation;
        }

        private void Shoot()
        {
            var instance = Instantiate(m_missilePrefab, m_muzzle.position, m_muzzle.rotation);
            Instantiate(m_shootEffect, m_muzzle.position, m_muzzle.rotation);
            SoundManager.Play(m_shootSound);
        }
    }
}
