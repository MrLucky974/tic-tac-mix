using LuckiusDev.Utils;
using RapidPrototyping.TicTacMix;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RapidPrototyping.Utils.Input
{
    [DefaultExecutionOrder(-1000)]
    [RequireComponent(typeof(PlayerInputManager)), DisallowMultipleComponent]
    public sealed class GameInputHandler : PersistentSingleton<GameInputHandler>
    {
        private const int DEFAULT_MAX_PLAYER_COUNT = 1;

        [SerializeField] private PlayerInputManager m_inputManager;
        [SerializeField] private InputActionAsset m_inputActions;
        [Space]
        [SerializeField] private InputListener m_inputListenerPrefab;

        private InputListener[] m_listeners;

        private void OnEnable()
        {
            InputSystem.onDeviceChange += OnDeviceChange;
        }

        private void OnDisable()
        {
            InputSystem.onDeviceChange -= OnDeviceChange;
        }

        protected override void Awake()
        {
            base.Awake();

            if (m_inputManager == null)
                m_inputManager = GetComponent<PlayerInputManager>();

            Debug.Assert(m_inputManager != null, $"{nameof(m_inputManager)} was not found!", this);

            var count = DEFAULT_MAX_PLAYER_COUNT;
            if (m_inputManager.maxPlayerCount != -1)
            {
                count = m_inputManager.maxPlayerCount;
            }

            m_listeners = new InputListener[count];
            if (m_inputListenerPrefab != null)
            {
                for (int i = 0; i < count; i++)
                {
                    var listener = Instantiate(m_inputListenerPrefab, transform);
                    m_listeners[i] = listener;
                    listener.Initialize();
                }
            }
            else
            {
                for (int i = 0; i < count; i++)
                {
                    var listener = InputListener.Create();
                    listener.transform.SetParent(transform);
                    m_listeners[i] = listener;
                    listener.Initialize(m_inputActions);
                }
            }
            
            DeterminePlayerControls();
        }

        private void Reset()
        {
            m_inputManager = GetComponent<PlayerInputManager>();
        }

        /// <summary>
        /// Handles device connection or disconnection events.
        /// </summary>
        /// <param name="device">The input device that changed.</param>
        /// <param name="change">The type of change (added, removed, etc.).</param>
        private void OnDeviceChange(InputDevice device, InputDeviceChange change)
        {
            if (device is Gamepad)
            {
                switch (change)
                {
                    case InputDeviceChange.Added:
                        Debug.Log("Gamepad connected: " + device.name);
                        DeterminePlayerControls();
                        break;

                    case InputDeviceChange.Removed:
                        Debug.Log("Gamepad disconnected: " + device.name);
                        DeterminePlayerControls();
                        break;

                    default:
                        break;
                }
            }
        }

        private void DeterminePlayerControls()
        {
            int gamepadCount = Gamepad.all.Count;
            int listenerCount = m_listeners.Length;
            if (gamepadCount > 0)
            {
                if (gamepadCount == listenerCount - 1)
                {
                    // Pair devices to listeners from 1 to listener count - 1
                    for (int i = 0; i < listenerCount; i++)
                    {
                        if (i == 0)
                        {
                            // First listener uses Keyboard
                            m_listeners[i].PairDevice(Keyboard.current);
                        }
                        else
                        {
                            // Remaining listeners use Gamepads
                            m_listeners[i].PairDevice(Gamepad.all[i - 1]);
                        }
                    }
                }
                else
                {
                    // Pair gamepads to every listener
                    for (int i = 0; i < listenerCount; i++)
                    {
                        if (i < gamepadCount)
                        {
                            m_listeners[i].PairDevice(Gamepad.all[i]);
                        }
                        else
                        {
                            // Remaining listeners use Keyboard
                            m_listeners[i].PairDevice(Keyboard.current);
                        }
                    }
                }
            }
            else
            {
                // No gamepads, assign all listeners to Keyboard
                foreach (var listener in m_listeners)
                {
                    listener.PairDevice(Keyboard.current);
                }
            }
        }

        public enum ActionMapIndex
        {
            Default = 0,
            TicTacToe = 1,
            Tanks     = 2,
        }

        public static void SetActionMap(ActionMapIndex index)
        {
            string mapIndex = "Default";

            switch (index)
            {
                case ActionMapIndex.TicTacToe:
                    mapIndex = "Tic Tac Toe";
                    break;
                case ActionMapIndex.Tanks:
                    mapIndex = "Tanks";
                    break;
            }

            foreach (var listener in Instance.m_listeners)
            {
                listener.SetInputMap(mapIndex);
            }
        }

        public static void Register(IPlayerControls player, int playerIndex = 0)
        {
            int listenerCount = Instance.m_listeners.Length;

            if (playerIndex < 0 && playerIndex >= listenerCount)
            {
                Debug.LogError($"Invalid playerIndex: {playerIndex}. It must be between 0 and {listenerCount - 1}.", Instance);
                return;
            }

            var listener = Instance.m_listeners[playerIndex];
            listener.Register(player);
        }

        public static void Unregister(int playerIndex = 0)
        {
            int listenerCount = Instance.m_listeners.Length;

            if (playerIndex < 0 && playerIndex >= listenerCount)
            {
                Debug.LogError("");
                return;
            }

            var listener = Instance.m_listeners[playerIndex];
            listener.Unregister();
        }

        public static void UnregisterAll()
        {
            foreach (var listener in Instance.m_listeners)
            {
                listener.Unregister();
            }
        }
    }
}
