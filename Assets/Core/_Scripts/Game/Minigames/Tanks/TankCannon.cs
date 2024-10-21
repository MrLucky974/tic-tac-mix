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
        [SerializeField] private Transform m_cannon;
        [SerializeField] private Transform m_muzzle;
        [SerializeField] private Missile m_missilePrefab;

        [Space(10)]

        [SerializeField] private float m_delay = 0.2f;

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
        }
    }
}
