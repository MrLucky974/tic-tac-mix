using UnityEngine;

namespace RapidPrototyping.TicTacMix.Targets
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private Rigidbody m_rigidbody;

        [Space]

        [SerializeField] private float m_moveSpeed = 5f;

        private int m_playerIndex;
        public int PlayerIndex => m_playerIndex;

        public void Initialize(int playerIndex)
        {
            m_playerIndex = playerIndex;
        }

        public void Fire(Vector3 direction)
        {
            m_rigidbody.AddRelativeForce(direction * m_moveSpeed);
        }

        private void OnCollisionEnter(Collision collision)
        {
            Destroy(gameObject);
        }
    }
}
