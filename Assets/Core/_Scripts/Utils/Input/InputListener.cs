using RapidPrototyping.TicTacMix;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;




#if UNITY_EDITOR
using UnityEditor;
#endif

namespace RapidPrototyping.Utils.Input
{
    [RequireComponent(typeof(PlayerInput)), DisallowMultipleComponent]
    public sealed class InputListener : MonoBehaviour
    {
        private const string MOVEMENT_ACTION_IDENTIFIER = "Movement";
        private const string PRIMARY_ACTION_IDENTIFIER = "Primary";

        [SerializeField] private PlayerInput m_playerInput;
        public PlayerInput PlayerInput
        {
            get { return m_playerInput; }
        }

        private GameObject m_playerControls;
        public GameObject PlayerControls
        {
            get { return m_playerControls; }
        }

        private InputActionMap m_currentActionMap;

        public void Initialize(InputActionAsset inputActions)
        {
            // NOTE: This method is called after Awake().
            Debug.Log($"Initialize() {name}");
            if (inputActions != null)
            {
                m_playerInput.actions = inputActions;
            }
            var index = m_playerInput.playerIndex + 1;

            // Set initial control scheme
            string controlScheme = $"Player {index}";
            m_playerInput.SwitchCurrentControlScheme(controlScheme);

            // Set initial action map
            SetInputMap("Default");
        }

        public void Initialize()
        {
            Initialize(null);
        }

        private void Awake()
        {
            if (m_playerInput == null)
                m_playerInput = GetComponent<PlayerInput>();

            Debug.Assert(m_playerInput != null, $"{nameof(m_playerInput)} was not found!", this);

            var index = m_playerInput.playerIndex + 1;
            name = $"InputListener_{index:000}";
            Debug.Log($"{name} is Awake()");

            m_playerInput.neverAutoSwitchControlSchemes = true;
            m_playerInput.notificationBehavior = PlayerNotifications.InvokeCSharpEvents;
        }

        private void OnEnable()
        {
            SubscribeToInputActions();
        }

        private void OnDisable()
        {
            UnsubscribeFromInputActions();
        }

        private void Reset()
        {
            m_playerInput = GetComponent<PlayerInput>();
            m_playerInput.neverAutoSwitchControlSchemes = true;
            m_playerInput.notificationBehavior = PlayerNotifications.InvokeCSharpEvents;
        }

        public void Register(GameObject player)
        {
            if (player == null)
            {
                Debug.LogError("Player is null. Ensure the player object is properly assigned before using it.", this);
                return;
            }

            m_playerControls = player;
        }

        public void SetInputMap(string actionMap)
        {
            // Unsubscribe from old action map
            UnsubscribeFromInputActions();

            // Switch to new action map
            m_playerInput.SwitchCurrentActionMap(actionMap);
            m_currentActionMap = m_playerInput.actions.FindActionMap(actionMap);

            // Subscribe to new action map
            SubscribeToInputActions();
        }

        public void PairDevice(InputDevice device)
        {
            InputUser.PerformPairingWithDevice(device, m_playerInput.user, InputUserPairingOptions.UnpairCurrentDevicesFromUser);
        }

        private void SubscribeToInputActions()
        {
            if (m_currentActionMap == null) return;

            var movementAction = m_currentActionMap.FindAction(MOVEMENT_ACTION_IDENTIFIER);
            var primaryAction = m_currentActionMap.FindAction(PRIMARY_ACTION_IDENTIFIER);

            if (movementAction != null)
            {
                movementAction.performed += OnMovementInternal;
                movementAction.canceled += OnMovementInternal;
            }

            if (primaryAction != null)
            {
                primaryAction.performed += OnPrimaryInternal;
                primaryAction.canceled += OnPrimaryInternal;
            }
        }

        private void UnsubscribeFromInputActions()
        {
            if (m_currentActionMap == null) return;

            var movementAction = m_currentActionMap.FindAction(MOVEMENT_ACTION_IDENTIFIER);
            var primaryAction = m_currentActionMap.FindAction(PRIMARY_ACTION_IDENTIFIER);

            if (movementAction != null)
            {
                movementAction.performed -= OnMovementInternal;
                movementAction.canceled -= OnMovementInternal;
            }

            if (primaryAction != null)
            {
                primaryAction.performed -= OnPrimaryInternal;
                primaryAction.canceled -= OnPrimaryInternal;
            }
        }

        private void OnMovementInternal(InputAction.CallbackContext ctx)
        {
            if (m_playerControls == null)
            {
                return;
            }

            var components = m_playerControls.GetComponentsInChildren<IPlayerMovementControls>();
            foreach (var component in components)
            {
                component?.OnMovement(ctx);
            }
        }

        private void OnPrimaryInternal(InputAction.CallbackContext ctx)
        {
            if (m_playerControls == null)
            {
                return;
            }

            var components = m_playerControls.GetComponentsInChildren<IPlayerPrimaryControls>();
            foreach (var component in components)
            {
                component?.OnPrimary(ctx);
            }
        }

        public static InputListener Create()
        {
            GameObject go = new();
            return go.AddComponent<InputListener>();
        }
    }

#if UNITY_EDITOR

    [CustomEditor(typeof(InputListener))]
    public class InputListenerInspector : Editor
    {
        private void OnEnable()
        {
            EditorApplication.update += OnUpdate;
        }

        private void OnDisable()
        {
            EditorApplication.update -= OnUpdate;
        }

        private void OnUpdate()
        {
            Repaint();
        }

        public override void OnInspectorGUI()
        {
            var listener = (InputListener)target;
            DrawDefaultInspector();

            // Add "Debug" header
            GUILayout.Space(10);
            GUILayout.Label("Debug", EditorStyles.boldLabel);

            // Create gray style
            GUIStyle grayStyle = new GUIStyle { normal = { textColor = Color.gray } };

            // Display "Current Action Map" and its name in one horizontal group
            string currentActionMapName = listener.PlayerInput.currentActionMap != null ? listener.PlayerInput.currentActionMap.name : "None";
            DrawHorizontalLabel("Current Action Map", currentActionMapName, grayStyle);

            GUILayout.Space(10);
            DrawHorizontalLabel("Current Control Scheme", listener.PlayerInput.currentControlScheme, grayStyle);

            // Display "Connected GameObject" and its value in one horizontal group
            var playerControlsGameObject = listener.PlayerControls;
            string gameObjectName = playerControlsGameObject != null ? playerControlsGameObject.name : "None";
            GUILayout.Space(10);
            DrawHorizontalLabel("Connected GameObject", gameObjectName, grayStyle);
        }

        // Helper method to draw two labels horizontally
        private void DrawHorizontalLabel(string label, string value, GUIStyle style)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(label, style);
            GUILayout.Label(value, style);
            GUILayout.EndHorizontal();
        }
    }

#endif
}
