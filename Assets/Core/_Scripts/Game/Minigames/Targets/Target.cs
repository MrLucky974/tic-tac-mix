using UnityEngine;

namespace RapidPrototyping.TicTacMix.Targets
{
    [RequireComponent(typeof(Collider))]
    public class Target : MonoBehaviour
    {
        [SerializeField] private int m_score = 1;
        [SerializeField] private Destructible m_destructible;

        private Collider m_collider;

        private void Start()
        {
            m_collider = GetComponent<Collider>();
        }

        public virtual void OnProjectileHit(int playerIndex)
        {
            GameManager.UpdateScore(m_score, playerIndex);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.TryGetComponent<Projectile>(out var projectile))
            {
                OnProjectileHit(projectile.PlayerIndex);

                if (m_destructible)
                {
                    m_destructible.Destroy();
                }
            }
        }
    }
}
