using UnityEngine;

namespace RapidPrototyping.TicTacMix.MysteryDoors
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private int m_playerIndex;
        [SerializeField] private SpriteRenderer m_spriteRenderer;

        [HideInInspector] public Floor currentFloor;

        private float m_offset;

        public void Initialize(int playerIndex, Floor floor)
        {
            currentFloor = floor;
            m_playerIndex = playerIndex;
            m_offset = playerIndex == 0 ? 0f : 1f;
            SetPosition(currentFloor.GetPosition(m_offset));
            m_spriteRenderer.color = playerIndex == 0 ? Color.blue : Color.red;
        }

        public void SetPosition(Vector3 position)
        {
            transform.position = position;
        }

        private void Update()
        {
            var deltaTime = Time.deltaTime;

            m_offset = Mathf.Clamp01(m_offset + 0.2f * deltaTime);
            Vector3 targetPosition = currentFloor.GetPosition(m_offset);
            //transform.position = targetPosition;
        }
    }
}
