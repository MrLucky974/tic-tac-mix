using LuckiusDev.Utils;
using UnityEngine;

namespace RapidPrototyping.TicTacMix
{
    public class InputManager : PersistentSingleton<InputManager>
    {
        [SerializeField] private GameInputActions m_inputActions;

        protected override void Awake()
        {
            base.Awake();
            m_inputActions = new GameInputActions();
            m_inputActions.Enable();
        }

        private void OnDestroy()
        {
            if (m_inputActions != null)
            {
                m_inputActions.Dispose();
            }
        }

        public static GameInputActions InputActions => Instance.m_inputActions;
    }
}
