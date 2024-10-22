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

        private int m_Score;
        private int m_beforChange = 5;

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
                    m_beforChange--;
                    Debug.Log(m_beforChange);
                    ChangingInput(m_beforChange);
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

        void ChangingInput(int current)
        {
            if (current <= 0)
            {
                m_p1Inputs = (Inputs)Random.Range(0, 4);
                Debug.Log(m_p1Inputs.ToString());

                m_beforChange = Random.Range(5, 11);
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
