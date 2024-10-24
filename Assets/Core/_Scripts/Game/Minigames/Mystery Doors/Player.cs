using UnityEngine;

namespace RapidPrototyping.TicTacMix.MysteryDoors
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private float m_speed = 0.2f;

        [Space]

        [SerializeField] private int m_playerIndex;
        [SerializeField] private SpriteRenderer m_spriteRenderer;

        [HideInInspector] public Stage currentStage;

        private float m_offset;

        public void Initialize(int playerIndex, Stage stage)
        {
            name = $"Player_{(playerIndex + 1)}";

            currentStage = stage;
            m_playerIndex = playerIndex;
            m_offset = playerIndex == 0 ? 0f : 1f;

            transform.position = currentStage.GetPosition(m_offset);
            m_spriteRenderer.color = playerIndex == 0 ? Color.blue : Color.red;

            GameManager.Instance.OnGameEnded += HandleGameEnd;
        }

        private void HandleGameEnd(GameManager.GameEndReason reason, int winIndex)
        {
            enabled = false;
        }

        public void SetNewStage(Stage stage)
        {
            currentStage = stage;
            m_offset = stage is Ground ? 0f : (m_playerIndex == 0 ? 0f : 1f);
            transform.position = currentStage.GetPosition(m_offset);
        }

        private void Update()
        {
            var deltaTime = Time.deltaTime;

            if (m_playerIndex == 0)
            {
                var input = InputManager.InputActions.P1Gameplay;
                var movement = input.Movement.ReadValue<Vector2>().x;
                m_offset = Mathf.Clamp01(m_offset + m_speed * movement * deltaTime);
            }
            else if (m_playerIndex == 1)
            {
                var input = InputManager.InputActions.P2Gameplay;
                var movement = input.Movement.ReadValue<Vector2>().x;
                m_offset = Mathf.Clamp01(m_offset + m_speed * movement * deltaTime);
            }

            Vector3 targetPosition = currentStage.GetPosition(m_offset);
            transform.position = targetPosition;
        }

        public int PlayerIndex => m_playerIndex;
    }
}
