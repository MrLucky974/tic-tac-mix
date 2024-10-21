using LuckiusDev.Utils;
using UnityEngine;

namespace RapidPrototyping.TicTacMix
{
    public class InputManager : PersistentSingleton<InputManager>
    {
        [SerializeField] private GameInputActions m_inputActions;

        private void Start()
        {
            m_inputActions = new GameInputActions();
            m_inputActions.Enable();
        }

        private void OnDestroy()
        {
            m_inputActions.Dispose();
        }

        public static GameInputActions InputActions => Instance.m_inputActions;
    }
}
