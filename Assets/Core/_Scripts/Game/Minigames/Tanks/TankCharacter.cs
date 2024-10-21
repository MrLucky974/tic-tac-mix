using UnityEngine;

namespace RapidPrototyping.TicTacMix.Tanks
{
    public struct CharacterInput
    {
        public float Movement;
        public Quaternion Rotation;
    }

    public class TankCharacter : MonoBehaviour
    {
        [SerializeField] private Rigidbody m_rigidbody;
        [SerializeField] private Transform m_body;

        private Quaternion m_requestedRotation;
        private Vector3 m_requestedMovement;

        public void Initialize(Transform cannon)
        {
            m_body.transform.rotation = cannon.rotation;
        }

        public void UpdateInput(CharacterInput input)
        {
            m_requestedMovement = new Vector3(0f, 0f, input.Movement);
            m_requestedMovement = Vector3.ClampMagnitude(m_requestedMovement, 1f);
            m_requestedMovement = m_body.transform.rotation * m_requestedMovement;

            m_requestedRotation = input.Rotation;
        }

        public void UpdateCharacter(float deltaTime)
        {
            UpdateRotation(deltaTime);
            UpdateVelocity(deltaTime);
        }

        public void UpdateVelocity(float deltaTime)
        {
            m_rigidbody.AddForce(m_requestedMovement * 10f * deltaTime, ForceMode.VelocityChange);
        }

        public void UpdateRotation(float deltaTime)
        {
            if (m_requestedMovement.magnitude <= 0f) return;

            var currentRotation = m_body.transform.rotation;

            var forward = Vector3.ProjectOnPlane
            (
                m_requestedRotation * Vector3.forward,
                Vector3.up
            );

            var targetRotation = Quaternion.LookRotation(forward, Vector3.up);
            currentRotation = Quaternion.Lerp
            (
                currentRotation,
                targetRotation,
                1f - Mathf.Exp(-10f * deltaTime)
            );
            m_body.transform.rotation = currentRotation;
        }
    }
}
