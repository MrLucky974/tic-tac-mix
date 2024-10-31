using CartoonFX;
using System;
using System.Collections;
using UnityEngine;

namespace RapidPrototyping.TicTacMix.Targets
{
    [RequireComponent(typeof(Collider))]
    public class Target : MonoBehaviour
    {
        [SerializeField] private float m_lifetime = 3f;
        [SerializeField] private int m_score = 1;
        [SerializeField] private Destructible m_destructible;

        [Space]

        [SerializeField] private CFXR_Effect m_disableEffect;
        [SerializeField] private CFXR_Effect m_impactEffect;

        public event Action OnTargetDestroyed;

        private Collider m_collider;
        private bool m_enabled = true;

        private void Start()
        {
            m_collider = GetComponent<Collider>();
            Invoke(nameof(Disable), m_lifetime);
        }

        private void Disable()
        {
            StartCoroutine(nameof(Rotate));
        }

        private IEnumerator Rotate()
        {
            Quaternion baseRotation = transform.rotation;
            Quaternion targetRotation = Quaternion.Euler(transform.eulerAngles
                + Vector3.up * 180f);

            float time = 0f;
            const float duration = 0.6f;
            while (time < duration)
            {
                time += Time.deltaTime;
                yield return null;

                float t = time / duration;

                // Rotate the target around
                transform.rotation = Quaternion.Lerp(baseRotation,
                    targetRotation, t);

                // If the animation is past half the duration and the collider
                // is still enabled, disable it
                if (t >= 0.5f && m_collider.enabled)
                {
                    m_collider.enabled = false;
                }
            }

            transform.rotation = targetRotation;
            yield return null;
            Instantiate(m_disableEffect, transform.position, Quaternion.identity);
            OnTargetDestroyed?.Invoke();
            Destroy(gameObject);
        }

        public virtual void OnProjectileHit(int playerIndex)
        {
            GameManager.UpdateScore(m_score, playerIndex);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (m_enabled is false)
                return;

            if (collision.gameObject.TryGetComponent<Projectile>(out var projectile))
            {
                OnProjectileHit(projectile.PlayerIndex);
                Instantiate(m_impactEffect, collision.contacts[0].point, Quaternion.identity);

                if (m_destructible)
                {
                    m_destructible.Destroy();
                }

                OnTargetDestroyed?.Invoke();
                m_enabled = false;
            }
        }
    }
}
