using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows;

namespace RapidPrototyping.TicTacMix.ArmWresling
{
    public class ArmWrestlingbehavior : MonoBehaviour
    {
        [SerializeField] private int m_playerIndex;
        private enum Inputs
        {
            UP,
            DOWN,
            LEFT,
            RIGHT
        }
        Dictionary<Inputs, Vector2> m_dictionary = new();

        private Inputs m_p1Inputs;
        private Inputs m_p2Inputs;

        private int m_Score = 0;
        private int m_beforChange;

        void Start()
        {
            InitDictionary();

            m_p1Inputs = Inputs.UP;
            m_p2Inputs = Inputs.RIGHT;
        }

        void Update()
        {
            if (m_playerIndex == 0)
            {
                var input = InputManager.InputActions.P1Gameplay;
                if (input.Movement.WasPressedThisFrame())
                {
                    var movement = input.Movement.ReadValue<Vector2>();
                    ValidInput(m_p1Inputs, movement);
                }
                
            }
            else
            {
                var input = InputManager.InputActions.P2Gameplay;
                var movement = input.Movement.ReadValue<Vector2>();
                ValidInput(m_p2Inputs, movement);
            }
        }
        void InitDictionary()
        {
            m_dictionary.Add(Inputs.UP, new Vector2(0, 1));
            m_dictionary.Add(Inputs.DOWN, new Vector2(0, -1));
            m_dictionary.Add(Inputs.LEFT, new Vector2(-1, 0));
            m_dictionary.Add(Inputs.RIGHT, new Vector2(1, 0));
        }

        void ChangeInput(int current, Inputs currentPlayerInput)
        {
            if (current == 0)
            {
                int newInput = Random.Range(0, 5);
                if (newInput == 0)
                {
                    currentPlayerInput = Inputs.LEFT;
                }
                if (newInput == 1)
                {
                    currentPlayerInput = Inputs.RIGHT;
                }
                if (newInput == 2)
                {
                    currentPlayerInput = Inputs.UP;
                }
                if (newInput == 3)
                {
                    currentPlayerInput = Inputs.DOWN;
                }
                m_beforChange = Random.Range(5, 10);
            }
        }
        void ValidInput(Inputs currentInput, Vector2 playerInput)
        {
            Vector2 value = new Vector2(0, 0);
            int count = 0;
            foreach (Inputs inputs in m_dictionary.Keys)
            {
                if (inputs == currentInput)
                {
                    value = m_dictionary.ElementAt(count).Value;
                }
                count++;
            }

            if (playerInput == value)
            {
                Debug.Log("yes right thing");
            }
        }
    }
}
