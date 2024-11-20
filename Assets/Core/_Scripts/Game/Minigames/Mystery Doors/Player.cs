using LuckiusDev.Utils;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

namespace RapidPrototyping.TicTacMix.MysteryDoors
{
    public class Player : MonoBehaviour, IPlayerControls
    {
        public enum AnimationState
        {
            IDLE = 0,
            WALK = 1,
            OPEN_DOOR = 2,
        }

        [Header("Settings")]
        [SerializeField] private int m_playerIndex;
        [SerializeField] private PlayerInput m_playerInput;

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
        [HideInInspector] public List<Door> hoveredDoors = new();
        private float m_offset;

        private AnimationState m_currentAnimationState = AnimationState.IDLE;
        private int m_animationSpriteIndex = 0;
        private float m_currentFrameDuration = 0f;

        #region Input Variables

        private Vector2 m_movementInput;
        private bool m_movementActionsPressedThisFrame;

        #endregion

        public void Initialize(int playerIndex, Stage stage)
        {
            name = $"Player_{(playerIndex + 1)}";

            currentStage = stage;
            m_playerIndex = playerIndex;
            m_offset = playerIndex == 0 ? 0f : 1f;

            m_playerInput.SwitchCurrentControlScheme($"Player {m_playerIndex + 1}");
            InputUser.PerformPairingWithDevice(Keyboard.current, m_playerInput.user, InputUserPairingOptions.None);
            InputUser.PerformPairingWithDevice(Mouse.current, m_playerInput.user, InputUserPairingOptions.None);
            if (m_playerIndex > 0)
            {
                if (Gamepad.all.Count >= m_playerIndex)
                {
                    var gamepad = Gamepad.all[m_playerIndex - 1];
                    InputUser.PerformPairingWithDevice(gamepad, m_playerInput.user, InputUserPairingOptions.None);
                }
            }

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

            var movement = m_movementInput.x;
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

            // Check if the up action was just pressed this frame
            if (m_movementActionsPressedThisFrame && m_movementInput.y > 0f)
            {
                // Check if the player is in front of any door
                if (hoveredDoors.Count > 0)
                {
                    // Sort the doors to get the closest one to the player
                    var doors = new List<Door>(hoveredDoors);
                    doors.Sort((a, b) =>
                    {
                        var aDistance = Vector2.Distance(a.transform.position, transform.position);
                        var bDistance = Vector2.Distance(b.transform.position, transform.position);

                        if (aDistance < bDistance)
                            return -1;

                        if (aDistance > bDistance)
                            return 1;

                        return 0;
                    });
                    
                    // Open the closest door
                    var door = doors.First();
                    door.OpenDoor(this);
                }
            }

            Vector3 targetPosition = currentStage.GetPosition(m_offset);
            transform.position = targetPosition;
        }

        private void LateUpdate()
        {
            m_movementActionsPressedThisFrame = false;
        }

        #region Player Controls Event Listeners

        public void OnMovement(InputAction.CallbackContext ctx)
        {
            m_movementInput = ctx.ReadValue<Vector2>();
            m_movementActionsPressedThisFrame = ctx.action.WasPressedThisFrame();
        }

        public void OnPrimary(InputAction.CallbackContext ctx)
        {
            // noop
        }

        #endregion

        #region Getters

        public int PlayerIndex => m_playerIndex;

        #endregion
    }
}
