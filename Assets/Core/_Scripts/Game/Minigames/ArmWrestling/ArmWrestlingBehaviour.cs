using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

namespace RapidPrototyping.TicTacMix.ArmWresling
{
    public class ArmWrestlingBehavior : MonoBehaviour, IPlayerControls
    {
        [Header("Input")]
        [SerializeField] private PlayerInput m_playerInput;
        [SerializeField] private int m_playerIndex;

        [Header("References")]
        [SerializeField] private ArmWreslingUIManager m_uiManager;
        [SerializeField] private ArmWreslingGameManager m_gameManager;
        
        public enum Inputs
        {
            UP,
            DOWN,
            LEFT,
            RIGHT
        }
        private readonly Inputs[] inputs =
        {
            Inputs.UP,
            Inputs.DOWN,
            Inputs.LEFT,
            Inputs.RIGHT
        };

        private readonly Dictionary<Inputs, Vector2> m_dictionary = new();

        private Inputs m_targetInput;
        private int m_remainingActions = 5;

        #region Input Variables

        private Vector2 m_movementInput;
        private bool m_movementPressedThisFrame;

        #endregion

        private void Start()
        {
            InitDictionary();

            m_playerInput.SwitchCurrentControlScheme(m_playerInput.defaultControlScheme);
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

            // Initialize the starting keys for each player
            m_targetInput = m_playerIndex == 0 ? Inputs.UP : Inputs.RIGHT;

            // Initialize the user interface
            m_uiManager.ShowRightIcon(m_targetInput, this);
        }

        private void Update()
        {
            if (m_movementPressedThisFrame)
            {
                if (ValidInput(m_movementInput))
                {
                    m_remainingActions--;
                    if (m_remainingActions <= 0)
                    {
                        m_remainingActions = Random.Range(5, 10);
                        int index = Random.Range(0, inputs.Length);
                        m_targetInput = inputs[index];
                        
                        m_uiManager.ShowRightIcon(m_targetInput, this);
                    }
                }
            }
        }

        private void LateUpdate()
        {
            m_movementPressedThisFrame = false;
        }

        private void InitDictionary()
        {
            m_dictionary.Add(Inputs.UP, Vector2.up);
            m_dictionary.Add(Inputs.DOWN, Vector2.down);
            m_dictionary.Add(Inputs.LEFT, Vector2.left);
            m_dictionary.Add(Inputs.RIGHT, Vector2.right);
        }

        public int GetPlayerIndex()
        {
            return m_playerIndex;
        }
        
        private bool ValidInput(Vector2 playerInput)
        {
            Vector2 value = Vector2.zero;
            foreach (Inputs input in m_dictionary.Keys)
            {
                if (input == m_targetInput)
                {
                    value = m_dictionary[input];
                    break;
                }
            }

            if (playerInput == value)
            {
                if (m_playerIndex == 0)
                {
                    Debug.Log("right input for player 1");
                    m_gameManager.IncreaseScore();
                    return true;
                }
                else if (m_playerIndex == 1)
                {
                    Debug.Log("right input for player 2");
                    m_gameManager.DecreaseScore();
                    return true;
                }
            }

            return false;
        }

        public void OnMovement(UnityEngine.InputSystem.InputAction.CallbackContext ctx)
        {
            m_movementInput = ctx.ReadValue<Vector2>();
            m_movementPressedThisFrame |= ctx.action.WasPressedThisFrame();
        }

        public void OnPrimary(UnityEngine.InputSystem.InputAction.CallbackContext ctx)
        {
            // noop
        }
    }
}
