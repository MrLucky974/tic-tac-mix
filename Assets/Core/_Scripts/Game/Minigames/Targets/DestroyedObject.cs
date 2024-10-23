using UnityEngine;

namespace RapidPrototyping.TicTacMix.Targets
{
    public class DestroyedObject : MonoBehaviour
    {
        [SerializeField] private bool m_explode = false;
        [SerializeField] private float radius = 5.0F;
        [SerializeField] private float power = 10.0F;

        [SerializeField] private Rigidbody[] m_bodies;

        private void Start()
        {
            if (m_explode)
            {
                foreach (var body in m_bodies)
                {
                    body.AddExplosionForce(power, transform.position, radius, 3.0F);
                }
            }
        }
    }
}
