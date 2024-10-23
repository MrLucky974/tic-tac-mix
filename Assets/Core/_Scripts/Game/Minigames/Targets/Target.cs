using UnityEngine;

namespace RapidPrototyping.TicTacMix.Targets
{
    [RequireComponent(typeof(Collider))]
    public class Target : MonoBehaviour
    {
        private Collider m_collider;

        private void Start()
        {
            m_collider = GetComponent<Collider>();
        }

        private void OnCollisionEnter(Collision collision)
        {
            Debug.Log(collision.gameObject);
        }
    }
}
