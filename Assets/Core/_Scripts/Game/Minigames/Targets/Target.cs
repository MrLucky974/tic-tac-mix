using UnityEngine;

namespace RapidPrototyping.TicTacMix.Targets
{
    [RequireComponent(typeof(Collider))]
    public class Target : MonoBehaviour
    {
        [SerializeField] private Destructible m_destructible;

        private Collider m_collider;

        private void Start()
        {
            m_collider = GetComponent<Collider>();
        }

        private void OnCollisionEnter(Collision collision)
        {
            Debug.Log(collision.gameObject);
            if (m_destructible)
            {
                m_destructible.Destroy();
            }
        }
    }
}
