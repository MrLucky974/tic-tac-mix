using LuckiusDev.Utils;
using UnityEngine;

namespace RapidPrototyping.TicTacMix.MysteryDoors
{
    public class Player : MonoBehaviour
    {
        public enum AnimationState
        {
            IDLE = 0,
            WALK = 1,
            OPEN_DOOR = 2,
        }

        [Header("Settings")]
        [SerializeField] private int m_playerIndex;

        [Space]

        [SerializeField] private Color m_blueColor;
        [SerializeField] private Color m_redColor;

        [Space]

        [SerializeField] private float m_speed = 0.2f;

        [Header("References")]
        [SerializeField] private SpriteRenderer m_spriteRenderer;

        [Header("Animations")]
        [SerializeField] private Sprite m_idleSprite;
        [SerializeField] private Sprite[] m_walkSprites;
        [SerializeField] private float m_walkFramePerSecond = 5f;
        [SerializeField] private Sprite m_doorSprite;

        [HideInInspector] public Stage currentStage;
        private float m_offset;

        private AnimationState m_currentAnimationState = AnimationState.IDLE;
        private int m_animationSpriteIndex = 0;
        private float m_currentFrameDuration = 0f;

        public void Initialize(int playerIndex, Stage stage)
        {
            name = $"Player_{(playerIndex + 1)}";

            currentStage = stage;
            m_playerIndex = playerIndex;
            m_offset = playerIndex == 0 ? 0f : 1f;

            transform.position = currentStage.GetPosition(m_offset);
            m_spriteRenderer.color = playerIndex == 0 ? m_blueColor : m_redColor;

            SetAnimationState(AnimationState.IDLE);

            GameManager.Instance.OnGameEnded += HandleGameEnd;
        }

        public AnimationState GetAnimationState()
        {
            return m_currentAnimationState;
        }

        public void SetAnimationState(AnimationState state)
        {
            if (state == m_currentAnimationState)
            {
                return;
            }

            m_currentAnimationState = state;

            m_currentFrameDuration = 0f;
            m_animationSpriteIndex = 0;
        }

        private void UpdateAnimation(float deltaTime)
        {
            m_currentFrameDuration += deltaTime;

            switch (m_currentAnimationState)
            {
                case AnimationState.IDLE:
                    m_spriteRenderer.sprite = m_idleSprite;
                    break;
                case AnimationState.WALK:
                    float walkFrameDelay = 1f / m_walkFramePerSecond;
                    if (m_currentFrameDuration > walkFrameDelay)
                    {
                        m_animationSpriteIndex = JMath.WrapValue(0,
                            m_walkSprites.Length - 1, m_animationSpriteIndex + 1);
                        m_currentFrameDuration = 0f;
                    }

                    m_spriteRenderer.sprite = m_walkSprites[m_animationSpriteIndex];
                    break;
                case AnimationState.OPEN_DOOR:
                    m_spriteRenderer.sprite = m_doorSprite;
                    break;
            }
        }

        private void HandleGameEnd(GameData data)
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
            UpdateAnimation(deltaTime);

            float normalizedSpeed = m_speed / currentStage.GetDistance();

            if (m_playerIndex == 0)
            {
                var input = InputManager.InputActions.P1Gameplay;
                var movement = input.Movement.ReadValue<Vector2>().x;

                m_offset = Mathf.Clamp01(m_offset + normalizedSpeed * movement * deltaTime);
                if (movement != 0f)
                {
                    m_spriteRenderer.flipX = movement < 0f;
                    if (m_currentAnimationState != AnimationState.OPEN_DOOR)
                    {
                        SetAnimationState(AnimationState.WALK);
                    }
                }
                else
                {
                    if (m_currentAnimationState != AnimationState.OPEN_DOOR)
                    {
                        SetAnimationState(AnimationState.IDLE);
                    }
                }
            }
            else if (m_playerIndex == 1)
            {
                var input = InputManager.InputActions.P2Gameplay;
                var movement = input.Movement.ReadValue<Vector2>().x;
                m_offset = Mathf.Clamp01(m_offset + normalizedSpeed * movement * deltaTime);
                if (movement != 0f)
                {
                    m_spriteRenderer.flipX = movement < 0f;

                    if (m_currentAnimationState != AnimationState.OPEN_DOOR)
                    {
                        SetAnimationState(AnimationState.WALK);
                    }
                }
                else
                {
                    if (m_currentAnimationState != AnimationState.OPEN_DOOR)
                    {
                        SetAnimationState(AnimationState.IDLE);
                    }
                }
            }

            Vector3 targetPosition = currentStage.GetPosition(m_offset);
            transform.position = targetPosition;
        }

        public int PlayerIndex => m_playerIndex;
    }
}
