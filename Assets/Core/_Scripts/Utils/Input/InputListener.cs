using RapidPrototyping.TicTacMix;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

namespace RapidPrototyping.Utils.Input
{
    [RequireComponent(typeof(PlayerInput)), DisallowMultipleComponent]
    public sealed class InputListener : MonoBehaviour, IPlayerControls
    {
        private const string MOVEMENT_ACTION_IDENTIFIER = "Movement";
        private const string PRIMARY_ACTION_IDENTIFIER = "Primary";

        [SerializeField] private PlayerInput m_playerInput;
        private IPlayerControls m_playerControls;

        public void Initialize(InputActionAsset inputActions)
        {
            m_playerInput.actions = inputActions;
            var index = m_playerInput.playerIndex + 1;
            m_playerInput.SwitchCurrentControlScheme($"Player {index}");
            Debug.Log($"Initialize() {name}");
        }

        private void Awake()
        {
            if (m_playerInput == null)
                m_playerInput = GetComponent<PlayerInput>();

            Debug.Assert(m_playerInput != null, $"{nameof(m_playerInput)} was not found!", this);

            var index = m_playerInput.playerIndex + 1;
            name = $"InputListener_{index:000}";

            m_playerInput.neverAutoSwitchControlSchemes = true;
            m_playerInput.notificationBehavior = PlayerNotifications.InvokeCSharpEvents;
            
            Debug.Log($"{name} is Awake()");
        }

        private void Start()
        {
            // Subscribe to input events
            SubscribeToInputActions();
        }

        private void OnDestroy()
        {
            UnsubscribeToInputActions();
        }

        private void Reset()
        {
            m_playerInput = GetComponent<PlayerInput>();
            m_playerInput.neverAutoSwitchControlSchemes = true;
        }

        public void Register(IPlayerControls player)
        {
            if (player == null)
            {
                Debug.LogError("Player is null. Ensure the player object is properly assigned before using it.", this);
                return;
            }

            m_playerControls = player;
        }

        public void Unregister()
        {
            m_playerControls = null;
        }

        public void SetInputMap(string actionMap)
        {
            m_playerInput.SwitchCurrentActionMap(actionMap);
        }

        public void PairDevice(InputDevice device)
        {
            InputUser.PerformPairingWithDevice(device, m_playerInput.user, InputUserPairingOptions.UnpairCurrentDevicesFromUser);
        }

        private void SubscribeToInputActions()
        {
            // Ensure actions exist before subscribing
            var movementAction = m_playerInput.actions[MOVEMENT_ACTION_IDENTIFIER];
            var primaryAction = m_playerInput.actions[PRIMARY_ACTION_IDENTIFIER];

            if (movementAction != null)
            {
                movementAction.performed += ctx => OnMovement(ctx);
                movementAction.canceled += ctx => OnMovement(ctx);
            }

            if (primaryAction != null)
            {
                primaryAction.performed += ctx => OnPrimary(ctx);
                primaryAction.canceled += ctx => OnPrimary(ctx);
            }
        }

        private void UnsubscribeToInputActions()
        {
            var movementAction = m_playerInput.actions[MOVEMENT_ACTION_IDENTIFIER];
            var primaryAction = m_playerInput.actions[PRIMARY_ACTION_IDENTIFIER];

            if (movementAction != null)
            {
                movementAction.performed -= ctx => OnMovement(ctx);
                movementAction.canceled -= ctx => OnMovement(ctx);
            }

            if (primaryAction != null)
            {
                primaryAction.performed -= ctx => OnPrimary(ctx);
                primaryAction.canceled -= ctx => OnPrimary(ctx);
            }
        }

        public void OnMovement(InputAction.CallbackContext ctx)
        {
            m_playerControls?.OnMovement(ctx);
        }

        public void OnPrimary(InputAction.CallbackContext ctx)
        {
            m_playerControls?.OnPrimary(ctx);
        }

        public static InputListener Create()
        {
            GameObject go = new();
            var listener = go.AddComponent<InputListener>();
            return listener;
        }
    }
}
