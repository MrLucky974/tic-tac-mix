using CartoonFX;
using UnityEngine;

namespace RapidPrototyping.TicTacMix.Tanks
{
    public class Missile : MonoBehaviour
    {
        [SerializeField] private float m_speed = 10f;
        [SerializeField] private int m_maximumBounces = 2;
        [SerializeField] private LayerMask m_wallMask;

        [Header("Effects")]
        [SerializeField] private CFXR_Effect m_bounceEffect;

        [Header("Audio")]
        [SerializeField] private AudioClip m_explosionSound;
        [SerializeField] private AudioClip m_bounceSound;

        private Vector3 m_velocity;
        private int m_bounceIndex = 0;

        private void Start()
        {
            m_velocity = transform.forward * m_speed;
        }

        private void Update()
        {
            // Limit how many times a missile can bounce off a wall
            if (m_bounceIndex >= m_maximumBounces)
            {
                SoundManager.Play(m_explosionSound);
                Destroy(gameObject);
                return;
            }

            // Perform a raycast to check for collisions
            RaycastHit hit;
            if (Physics.Raycast(transform.position, m_velocity, out hit, m_velocity.magnitude * Time.deltaTime, m_wallMask))
            {
                // Calculate the reflected direction
                Vector3 reflectedDirection = Vector3.Reflect(m_velocity.normalized, hit.normal);
                m_velocity = reflectedDirection * m_speed;
                transform.forward = reflectedDirection;

                Instantiate(m_bounceEffect, transform.position, Quaternion.identity);
                m_bounceIndex += 1;
                SoundManager.Play(m_bounceSound);
            }

            // Update the position of the projectile
            transform.position += m_velocity * Time.deltaTime;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (other.TryGetComponent(out Health health))
                {
                    health.Kill();
                }

                SoundManager.Play(m_explosionSound);
                Destroy(gameObject);
            }
        }
    }
}
